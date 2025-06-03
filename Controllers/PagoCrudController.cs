using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;
using Microsoft.AspNetCore.Authorization;

namespace SweetNela.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagoCrudController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoCrudController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PagoCrud
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DbSetPago.Include(p => p.Order).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PagoCrud/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.DbSetPago
                .Include(p => p.Order)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: PagoCrud/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.DbSetOrden, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: PagoCrud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MontoTotal,PaymentDate,Status,UserId,OrderId,PayPalPaymentId,PayPalPayerId")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.DbSetOrden, "Id", "Id", pago.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", pago.UserId);
            return View(pago);
        }

        // GET: PagoCrud/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.DbSetPago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.DbSetOrden, "Id", "Id", pago.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", pago.UserId);
            return View(pago);
        }

        // POST: PagoCrud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MontoTotal,PaymentDate,Status,UserId,OrderId,PayPalPaymentId,PayPalPayerId")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["OrderId"] = new SelectList(_context.DbSetOrden, "Id", "Id", pago.OrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", pago.UserId);
            return View(pago);
        }

        // GET: PagoCrud/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.DbSetPago
                .Include(p => p.Order)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: PagoCrud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.DbSetPago.FindAsync(id);
            if (pago != null)
            {
                _context.DbSetPago.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.DbSetPago.Any(e => e.Id == id);
        }
    }
}
