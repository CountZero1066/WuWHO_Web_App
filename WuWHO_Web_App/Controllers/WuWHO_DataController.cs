using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WuWHO_Web_App.Data;
using WuWHO_Web_App.Models;
using System.Web;


//Besides the 'Count_devices' and 'Chart_examples' controller actions, the rest were automatically generated via scaffolding

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
            //The following controller action queries the remote MySQL database via LINQ queries, these query results are displayed on the 'Count_Devices' view

            //The first query simply counts all records in the database table, excluding any "end of statement" entries
            int total_detect = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Count();
            ViewBag.total_num = total_detect;

            //Counts the total number of distinct MAC IDs recorded in the database
            int Unique_devices = _context.tbl_environment_4.Select(m => m.MAC_ID).Distinct().Count();
            ViewBag.the_count = Unique_devices;

            // Counts the number of distinct devices detected within the last five minutes
            var FiveMinAgo = DateTime.Now.AddHours(-0.1);
            ViewBag.devices_in_last_5_minutes = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Where(m => m.time_rec >= FiveMinAgo).Select(m=>m.MAC_ID).Distinct().Count();

           
            return View();
        }


        public IActionResult Chart_example()
        {
            //The following LINQ queries are used in teh Chart_example view and are sued in the Highchart graphs
            //The first query counts the number of MAC detections that are timestamped as belonging to today and groups them by the hour associated with them 
            var my_data_today = _context.tbl_environment_4.Where(m => m.time_rec.Date == DateTime.Today).Where(m => m.MAC_ID != "end of statement").GroupBy(m => m.time_rec.Hour).Select(g => new MyClass{name = g.Key, count = g.Count() }).ToList();
            ViewBag.todays_data = my_data_today;

            //The second query is identical to the first excempt it counts and groups all of yesterdays detections
            var my_data_yesterday = _context.tbl_environment_4.Where(m => m.time_rec.Date == DateTime.Today.AddDays(-1)).Where(m => m.MAC_ID != "end of statement").GroupBy(m => m.time_rec.Hour).Select(g => new MyClass { name = g.Key, count = g.Count() }).ToList();
            ViewBag.yesterdays_data = my_data_yesterday;

            //The third query is used in the Highchart gauge and infers the number of Unique devices detected in the last five minutes
            var FiveMinAgo = DateTime.Now.AddHours(-0.1);
            ViewBag.devices_in_last_5_minutes = _context.tbl_environment_4.Where(m => m.MAC_ID != "end of statement").Where(m => m.time_rec >= FiveMinAgo).Select(m => m.MAC_ID).Distinct().Count();

            return View("/Views/Chart_data/Chart_example.cshtml");
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
