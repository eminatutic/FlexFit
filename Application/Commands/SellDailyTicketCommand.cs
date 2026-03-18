using FlexFit.Application.DTOs;
using MediatR;

namespace FlexFit.Application.Commands
{
    public class SellDailyTicketCommand : IRequest<bool>
    {
        public SellDailyTicketDto Dto { get; }

        public SellDailyTicketCommand(SellDailyTicketDto dto)
        {
            Dto = dto;
        }
    }
}
