using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PurchasesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => 
        Ok(await _context.Purchases.Include(p => p.Supplier).Include(p => p.PurchaseItems).ThenInclude(pi => pi.Product).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var purchase = await _context.Purchases.Include(p => p.Supplier).Include(p => p.PurchaseItems).ThenInclude(pi => pi.Product).FirstOrDefaultAsync(p => p.Id == id);
        return purchase == null ? NotFound() : Ok(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Purchase purchase)
    {
        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = purchase.Id }, purchase);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Purchase purchase)
    {
        if (id != purchase.Id) return BadRequest();
        _context.Entry(purchase).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var purchase = await _context.Purchases.Include(p => p.PurchaseItems).FirstOrDefaultAsync(p => p.Id == id);
        if (purchase == null) return NotFound();
        _context.PurchaseItems.RemoveRange(purchase.PurchaseItems);
        _context.Purchases.Remove(purchase);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
