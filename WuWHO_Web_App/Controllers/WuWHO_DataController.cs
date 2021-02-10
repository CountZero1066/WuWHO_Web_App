using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WuWHO_Web_App.Data;
using WuWHO_Web_App.Models;


namespace WuWHO_Web_App.Controllers
{
    public class WuWHO_DataController : Controller
    {
        private readonly WuWHO_Context _context;

        public WuWHO_DataController(WuWHO_Context context)
        {
            _context = context;
        }

        
        [Route("/Count")]
        public IActionResult Count_devices()
        {
            int total_detect = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Count();
            ViewBag.total_num = total_detect;

            int Unique_devices = _context.tbl_environment_4.Select(m => m.MAC_ID).Distinct().Count();
            ViewBag.the_count = Unique_devices;

            var FiveMinAgo = DateTime.Now.AddHours(-0.1);
            ViewBag.devices_in_last_5_minutes = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Where(m => m.time_rec >= FiveMinAgo).Select(m=>m.MAC_ID).Distinct().Count();


            
            int num_devices = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Where(m => m.time_rec >= FiveMinAgo).Select(m => m.MAC_ID).Distinct().Count();
            string tweet_body = "";
            tweet_body = "Total number of unique Device detections = " + Unique_devices.ToString();
           
            TwitterPost.Sendtweet(tweet_body);

            return View();
        }
        
        
        // GET: WuWHO_Data
        public async Task<IActionResult> Index()
        {
            return View(await _context.tbl_environment_4.ToListAsync());
        }

        
        
        // GET: WuWHO_Data/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wuWHO_db = await _context.tbl_environment_4
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wuWHO_db == null)
            {
                return NotFound();
            }

            return View(wuWHO_db);
        }

        // GET: WuWHO_Data/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WuWHO_Data/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MAC_ID,RSSI,time_rec")] WuWHO_db wuWHO_db)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wuWHO_db);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wuWHO_db);
        }

        // GET: WuWHO_Data/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wuWHO_db = await _context.tbl_environment_4.FindAsync(id);
            if (wuWHO_db == null)
            {
                return NotFound();
            }
            return View(wuWHO_db);
        }

        // POST: WuWHO_Data/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MAC_ID,RSSI,time_rec")] WuWHO_db wuWHO_db)
        {
            if (id != wuWHO_db.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wuWHO_db);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WuWHO_dbExists(wuWHO_db.ID))
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
            return View(wuWHO_db);
        }

        // GET: WuWHO_Data/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wuWHO_db = await _context.tbl_environment_4
                .FirstOrDefaultAsync(m => m.ID == id);
            if (wuWHO_db == null)
            {
                return NotFound();
            }

            return View(wuWHO_db);
        }

        // POST: WuWHO_Data/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wuWHO_db = await _context.tbl_environment_4.FindAsync(id);
            _context.tbl_environment_4.Remove(wuWHO_db);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WuWHO_dbExists(int id)
        {
            return _context.tbl_environment_4.Any(e => e.ID == id);
        }
    }
}
