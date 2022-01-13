using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tr3Line.Assessment.Api.Entities;
using Tr3Line.Assessment.Api.Models.Accounts;
using Tr3Line.Assessment.Api.Services;
using Tr3Line.Assessment.Api.Services.Interface;
using Tr3Line.Assessment.Api.Models.Accounts.MobileApp;
using System.Threading.Tasks;
using Tr3Line.Assessment.Api.Repository.Interface;
using System.Linq;
using Tr3Line.Assessment.Api.Models.Bank;

namespace Tr3Line.Assessment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ATMCardController : BaseController
    {
        private readonly IMailingServices _emailService;
        private readonly IATMCardRepository _aTMCard;
        private readonly IMapper _mapper;

        public ATMCardController(
            IMailingServices emailService,
            IATMCardRepository aTMCard,
            IMapper mapper)
        {
            _emailService = emailService;
            _mapper = mapper;
            _aTMCard = aTMCard;
        }


        [Authorize]
        [HttpGet("GetCardsByUserId/{id}")]
        public  ActionResult GetCardsByUserId(int id)
        {
            try
            {
                if(Account == null)
                {
                    return NotFound(new { message = "please login." });
                }
                var response = _aTMCard.GetATMCardsByUserId(id);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = "resource not found" });
            }
        }

        [Authorize]
        [HttpGet("GetCardsByBankAccountId/{id}")]
        public ActionResult GetCardsByBankAccountId(int id)
        {
            try
            {
                if (Account == null)
                {
                    return NotFound(new { message = "please login." });
                }
                var response = _aTMCard.GetATMCardsByBankAccountId(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "resource not found" });
            }
        }

        [Authorize]
        [HttpGet("GetCardById/{id}")]
        public async Task<ActionResult> GetCardById(int id)
        {
            try
            {
                if (Account == null)
                {
                    return NotFound(new { message = "please login." });
                }
                var response = await _aTMCard.GetByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "resource not found" });
            }
        }


        [Authorize]
        [HttpPost("AddCardByUserId")]
        public async Task<ActionResult> AddCardByUserId(ATMCardViewModel aTMCard)
        {
            try
            {
                if (Account == null)
                {
                    return NotFound(new { message = "please login." });
                }

                ATMCard saveaTMCard = new ATMCard()
                {
                    CardAddress = aTMCard.CardAddress,
                    CardName = aTMCard.CardName,
                    CardNumber = aTMCard.CardNumber,
                    ExpireDate = aTMCard.ExpireDate,
                    CVV = aTMCard.CVV
                };

                saveaTMCard.IsActive = true;
                saveaTMCard.UserId = Account.Id;
                saveaTMCard.DateAdded = DateTime.Now;
                await _aTMCard.CreateAsync(saveaTMCard);

                await _aTMCard.SaveChangesAsync();
                return Ok(new { message = "card was added successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "resource not found" });
            }
        }

        [Authorize]
        [HttpDelete("RemoveCard/{cardId}")]
        public async Task<ActionResult> RemoveCard(int cardId)
        {
            try
            {
                if (Account == null)
                {
                    return NotFound(new { message = "please login." });
                }

                var getAtmCrd = await _aTMCard.GetByIdAsync(cardId);
                if (Account.Id != getAtmCrd.UserId)
                {
                    return NotFound(new { message = "this card is not for you." });
                }

                 _aTMCard.Delete(getAtmCrd);
                await _aTMCard.SaveChangesAsync();
                return Ok(new { message = "card was added successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = "resource not found" });
            }
        }


        #region helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        #endregion
    }
}
