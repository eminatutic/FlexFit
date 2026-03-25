using FlexFit.Application.DTOs;
using MediatR;

namespace FlexFit.Application.Commands
{
    public class CancelPenaltyCommand : IRequest<bool>
    {
        public string Id { get; }
        public CancelPenaltyDto Dto { get; }

        public CancelPenaltyCommand(string id, CancelPenaltyDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
