using Microsoft.AspNetCore.Mvc;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQsController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        /// <summary>
        /// Busca FAQs relevantes baseadas em uma descrição
        /// GET /api/faqs/buscar?descricao=problema+com+impressora
        /// </summary>
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarFAQs([FromQuery] string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(descricao))
                {
                    return BadRequest("Descrição é obrigatória");
                }

                var faqs = await _faqService.BuscarFAQsRelevantesAsync(descricao, 3);
                return Ok(faqs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Marca uma FAQ como útil (incrementa relevância)
        /// POST /api/faqs/{id}/marcar-util
        /// </summary>
        [HttpPost("{id}/marcar-util")]
        public async Task<IActionResult> MarcarComoUtil(int id)
        {
            try
            {
                await _faqService.MarcarFAQComoUtilAsync(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lista FAQs populares
        /// GET /api/faqs/populares
        /// </summary>
        [HttpGet("populares")]
        public async Task<IActionResult> ListarPopulares([FromQuery] int limite = 10)
        {
            try
            {
                var faqs = await _faqService.ListarFAQsPopularesAsync(limite);
                return Ok(faqs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
