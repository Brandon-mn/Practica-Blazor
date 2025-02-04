using MudBlazorWebApp1.Data;
using MudBlazorWebApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace MudBlazorWebApp1.Services
{
    public class TicketTemplateService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TicketTemplateService> logger;

        public TicketTemplateService(AppDbContext context, ILogger<TicketTemplateService> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<List<TicketTemplate>> GetAllTemplatesAsync()
        {
            return await _context.TicketTemplates2
                .Include(t => t.Elements)
                .ToListAsync();
        }

        public async Task<TicketTemplate?> GetTemplateByIdAsync(int id)
        {
            return await _context.TicketTemplates2
                .Include(t => t.Elements)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<DatosEvento?> GetDatosEventoByTemplateIdAsync(int templateId)
        {
            try
            {
                return await _context.DatosEventos
                    .FirstOrDefaultAsync(e => e.Id == 1); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching event data");
                throw;
            }
        }


        public async Task<TicketTemplate> CreateTemplateAsync(TicketTemplate template)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.TicketTemplates2.Add(template);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return template;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating template");
                throw;
            }
        }

        public async Task<bool> UpdateElementAsync(TicketElement element)
        {
            try
            {
                _context.TicketElements.Update(element);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating element");
                throw;
            }
        }

        public async Task AddElementToTemplateAsync(int templateId, TicketElement newElement)
        {
            var template = await _context.TicketTemplates2
                .FindAsync(templateId);

            if (template != null)
            {
                newElement.TicketTemplateId = templateId;
                _context.TicketElements.Add(newElement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteElementAsync(int elementId)
        {
            var element = await _context.TicketElements.FindAsync(elementId);
            if (element != null)
            {
                _context.TicketElements.Remove(element);
                await _context.SaveChangesAsync();
            }
        }
    }
}