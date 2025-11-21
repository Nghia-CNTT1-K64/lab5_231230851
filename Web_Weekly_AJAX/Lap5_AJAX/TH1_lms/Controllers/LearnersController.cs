using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TH1_lms.Models;

namespace TH1_lms.Controllers
{
    public class LearnersController : Controller
    {
        private readonly SchoolContext _context;

        public LearnersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Learners
        private int pageSize = 5; // Khai báo biến toàn cục pageSize

        public async Task<IActionResult> Index(int? mid, string searchString, int? pageIndex)
        {
            // Lấy toàn bộ learners trong dbset chuyển về IQueryable<Learner> để query
            var learners = _context.Learners
                .Include(m => m.Major)
                .AsQueryable();

            // Lấy chỉ số trang, nếu chỉ số trang null thì gán ngầm định bằng 1
            int page = pageIndex ?? 1;

            // Nếu có mid thì lọc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                // Lọc
                learners = learners.Where(l => l.MajorID == mid);
                // Gửi mid về view để ghi lại trên nav-phân trang
                ViewBag.mid = mid;
            }

            // Nếu có keyword thì tìm kiếm theo tên
            if (searchString != null)
            {
                // Tìm kiếm
                learners = learners.Where(l => l.FirstMidName.ToLower()
                    .Contains(searchString.ToLower()) ||
                    l.LastName.ToLower().Contains(searchString.ToLower()));
                // Gửi keyword về view để ghi lại trên nav-phân trang
                ViewBag.searchString = searchString;
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            // Trả số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Gửi chỉ số trang về view để hiển thị trên nav-phân trang
            ViewBag.pageIndex = page;

            // Chọn dữ liệu trong trang hiện tại
            var result = await learners
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();

            return View(result);
        }

        public IActionResult LearnerFilter(int? mid, string keyword, int? pageIndex)
        {
            // Lấy toàn bộ learners trong dbset chuyển về IQueryable<Learner> để query
            var learners = _context.Learners
                .Include(m => m.Major)
                .AsQueryable();

            // Lấy chỉ số trang, nếu chỉ số trang null thì gán ngầm định bằng 1
            int page = pageIndex ?? 1;
            if (pageIndex <= 0) page = 1;

            // Nếu có mid thì lọc learner theo mid (chuyên ngành)
            if (mid != null)
            {
                // Lọc
                learners = learners.Where(l => l.MajorID == mid);
                // Gửi mid về view để ghi lại trên nav-phân trang
                ViewBag.mid = mid;
            }

            // Nếu có keyword thì tìm kiếm theo tên
            if (keyword != null)
            {
                // Tìm kiếm
                learners = learners.Where(l => l.FirstMidName.ToLower()
                    .Contains(keyword.ToLower()) ||
                    l.LastName.ToLower().Contains(keyword.ToLower()));
                // Gửi keyword về view để ghi lại trên nav-phân trang
                ViewBag.keyword = keyword;
            }

            // Tính số trang
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            // Trả số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // Gửi chỉ số trang về view để hiển thị trên nav-phân trang
            ViewBag.pageIndex = page;

            // Chọn dữ liệu trong trang hiện tại
            var result = learners
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(m => m.Major)
                .ToList();

            return PartialView("LearnerTable", result);
        }


        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = _context.Learners
                .Where(l => l.MajorID == mid)
                .Include(l => l.Major)
                .ToList();

            return PartialView("LearnerTable", learners);
        }


        // GET: Learners/Create
        public IActionResult Create()
        {
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName");
            return View();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerID,LastName,FirstMidName,EnrollmentDate,MajorID")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerID,LastName,FirstMidName,EnrollmentDate,MajorID")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
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
            ViewData["MajorID"] = new SelectList(_context.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.Major)
                .FirstOrDefaultAsync(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                _context.Learners.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerID == id);
        }
    }
}
