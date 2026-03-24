using FlexFit.Application.DTOs;
using MediatR;

namespace FlexFit.Application.Commands
{
    public class LogEntryCommand : IRequest<bool>
    {
        public LogEntryDto Dto { get; }

        public LogEntryCommand(LogEntryDto dto)
        {
            Dto = dto;
        }
    }
}
