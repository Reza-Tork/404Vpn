using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Bot;
using Infrastructure.DbContext;

namespace UI.Web.Controllers
{
    public class BotSettingsController : Controller
    {
        private readonly BotDbContext _context;

        public BotSettingsController(BotDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BotSettings.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Key,Value")] BotSetting botSetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(botSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(botSetting);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var botSetting = await _context.BotSettings.FindAsync(id);
            if (botSetting == null)
            {
                return NotFound();
            }
            return View(botSetting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Key,Value")] BotSetting botSetting)
        {
            if (id != botSetting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(botSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BotSettingExists(botSetting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(botSetting);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var botSetting = await _context.BotSettings.FindAsync(id);
            if (botSetting != null)
            {
                _context.BotSettings.Remove(botSetting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BotSettingExists(int id)
        {
            return _context.BotSettings.Any(e => e.Id == id);
        }
    }
}
