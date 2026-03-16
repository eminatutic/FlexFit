using FlexFit.Models;
using FlexFit.MongoModels.Models;
using FlexFit.MongoModels.Repositories;
using FlexFit.UnitOfWorkLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/employee")]
[Authorize(Roles = "Employee")]
public class EmployeeController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly EntryLogRepository _entryLogRepo;
    private readonly IncidentRepository _incidentRepo;

    public EmployeeController(IUnitOfWork unitOfWork, EntryLogRepository entryLogRepo, IncidentRepository incidentRepo)
    {
        _unitOfWork = unitOfWork;
        _entryLogRepo = entryLogRepo;
        _incidentRepo = incidentRepo;
    }

    [HttpPost("scan")]
    public async Task<IActionResult> ScanCard(int memberId, int fitnessId)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(memberId);
        if (member == null)
            return NotFound("Member not found");

        var activeSubscription = (await _unitOfWork.MembershipCards.GetAllAsync())
                                    .OfType<SubscriptionCard>()
                                    .FirstOrDefault(c => c.MemberId == memberId && c.ValidTo > DateTime.UtcNow);

        bool incident = false;
        string cardStatus = "Valid";

        if (activeSubscription == null)
        {
            incident = true;
            cardStatus = "Invalid";

            var incidentLog = new Incident
            {
                MemberId = memberId,
                FitnessObjectId = fitnessId,
                Time = DateTime.UtcNow,
                Reason = "Unauthorized entry attempt"
            };
            await _incidentRepo.AddAsync(incidentLog);
        }

        var log = new EntryLog
        {
            MemberId = memberId,
            EmployeeId = 0, // ID zaposlenog iz JWT
            FitnessObjectId = fitnessId,
            Time = DateTime.UtcNow,
            CardStatus = cardStatus,
            Incident = incident
        };
        await _entryLogRepo.AddAsync(log);

        return Ok(new { Status = cardStatus, Incident = incident });
    }
}