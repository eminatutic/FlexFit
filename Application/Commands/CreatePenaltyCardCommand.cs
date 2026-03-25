using FlexFit.Application.DTOs;
using FlexFit.Domain.Models;
using MediatR;

namespace FlexFit.Application.Commands
{
    public class CreatePenaltyCardCommand : IRequest<bool>
    {
        public CreatePenaltyCardDto Dto { get; }
        public CreatePenaltyCardCommand(CreatePenaltyCardDto dto) => Dto = dto;
    }
}
