using FlexFit.Application.DTOs;
using MediatR;

namespace FlexFit.Application.Commands
{
    public class CreatePenaltyPointCommand : IRequest<bool>
    {
        public CreatePenaltyPointDto Dto { get; }
        public CreatePenaltyPointCommand(CreatePenaltyPointDto dto) => Dto = dto;
    }
}
