namespace FlexFit.Infrastructure.Repositories.Interfaces
{
    public interface IMemberGraphRepository
    {
        Task RecordVisitAsync(string memberId, int fitnessObjectId, string memberName = null, string objectName = null);
        Task RecordReservationAsync(string memberId, int resourceId, string memberName = null, string resourceType = null);
        Task<IEnumerable<string>> GetRecommendedObjectsAsync(string memberId);
        
        Task AssignCardToMemberAsync(string memberId, string cardId, string cardName = null, bool isActive = false);
        Task RecordBookingAsync(string memberId, int resourceId, string bookingId = null);
        Task RecordEmployeeCheckAsync(string employeeId, string memberId, string employeeName = null);
        Task RecordCardCheckAsync(string employeeId, string cardId, string employeeName = null);
        Task CreateCardAsync(string cardId, string cardName);
        Task AssignPenaltyToMemberAsync(string penaltyId, string memberId, string penaltyDescription = null);
        Task LinkResourceToFitnessObjectAsync(int resourceId, int fitnessObjectId, string resourceName = null, string fitnessObjectName = null);
        Task LinkCardToGymAsync(string cardId, int gymId);
    }
}
