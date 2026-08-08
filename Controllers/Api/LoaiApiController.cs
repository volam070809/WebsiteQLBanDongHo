using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebsiteQLBanDongHo.WebsiteQLBanDongHoDomain.DataContext;

namespace WebsiteQLBanDongHo.Controllers.Api
{
    // NOTE: Đây là MVC Controller trả JSON (không dùng ASP.NET WebApi) để tránh lỗi package/version.
    // Endpoint:
    //   GET    /api/loai
    //   GET    /api/loai/{id}
    //   POST   /api/loai        (JSON: { "tenLoai": "..." } hoặc { "maLoai": "...", "tenLoai": "..." })
    //   PUT    /api/loai/{id}   (JSON: { "tenLoai": "..." })
    //   DELETE /api/loai/{id}

    [RoutePrefix("api/loai")]
    public class LoaiApiController : Controller
    {
        private readonly WebsiteQLBanDongHoEntities db = new WebsiteQLBanDongHoEntities();

        public class LoaiDto
        {
            public string MaLoai { get; set; }
            public string TenLoai { get; set; }
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetAll()
        {
            var data = db.LOAISANPHAMs
                .Select(x => new
                {
                    maLoai = x.MALOAISP.Trim(),
                    tenLoai = x.TENLOAISP
                })
                .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetById(string id)
        {
            id = (id ?? "").Trim();
            var x = db.LOAISANPHAMs.FirstOrDefault(t => t.MALOAISP.Trim() == id);
            if (x == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Không tìm thấy loại");

            return Json(new { maLoai = x.MALOAISP.Trim(), tenLoai = x.TENLOAISP }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("")]
        public ActionResult Create(LoaiDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TenLoai))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Tên loại không được rỗng");

            var newId = string.IsNullOrWhiteSpace(dto.MaLoai)
                ? GenerateNextMaLoai()
                : dto.MaLoai.Trim();

            if (newId.Length > 7)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Mã loại tối đa 7 ký tự (char(7))");

            var exists = db.LOAISANPHAMs.Any(x => x.MALOAISP.Trim() == newId);
            if (exists)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Mã loại đã tồn tại");

            var entity = new LOAISANPHAM
            {
                MALOAISP = newId,
                TENLOAISP = dto.TenLoai.Trim()
            };

            db.LOAISANPHAMs.Add(entity);
            db.SaveChanges();

            Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new { maLoai = entity.MALOAISP.Trim(), tenLoai = entity.TENLOAISP });
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(string id, LoaiDto dto)
        {
            id = (id ?? "").Trim();
            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Thiếu mã loại");

            if (dto == null || string.IsNullOrWhiteSpace(dto.TenLoai))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Tên loại không được rỗng");

            var entity = db.LOAISANPHAMs.FirstOrDefault(x => x.MALOAISP.Trim() == id);
            if (entity == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Không tìm thấy loại");

            entity.TENLOAISP = dto.TenLoai.Trim();
            db.SaveChanges();

            return Json(new { maLoai = entity.MALOAISP.Trim(), tenLoai = entity.TENLOAISP });
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(string id)
        {
            id = (id ?? "").Trim();
            var entity = db.LOAISANPHAMs.FirstOrDefault(x => x.MALOAISP.Trim() == id);
            if (entity == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Không tìm thấy loại");

            try
            {
                db.LOAISANPHAMs.Remove(entity);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // Thường là do FK (đang có sản phẩm/chi tiết sản phẩm dùng loại này)
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Không thể xóa vì đang được sử dụng ở bảng khác");
            }

            Response.StatusCode = (int)HttpStatusCode.NoContent;
            return Content(string.Empty);
        }

        private string GenerateNextMaLoai()
        {
            // MALOAISP là char(7) => cố gắng sinh mã dựa trên pattern hiện có.
            var codes = db.LOAISANPHAMs
                .Select(x => x.MALOAISP)
                .ToList()
                .Select(s => (s ?? "").Trim())
                .Where(s => s.Length > 0)
                .ToList();

            if (codes.Count == 0)
                return "LSP0001"; // 7 ký tự

            // Lấy prefix chữ ở đầu cho 1 code mẫu (tới khi gặp số)
            var sample = codes[0];
            int prefixLen = 0;
            while (prefixLen < sample.Length && !char.IsDigit(sample[prefixLen]))
                prefixLen++;

            var prefix = prefixLen > 0 ? sample.Substring(0, prefixLen) : "LSP";
            int numLen = Math.Max(1, 7 - prefix.Length);

            int maxNum = 0;
            foreach (var c in codes)
            {
                var cc = c;
                if (!cc.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                var part = cc.Substring(prefix.Length);
                // Nếu part có space do char(7), trim nữa
                part = part.Trim();
                if (part.Length == 0)
                    continue;

                if (int.TryParse(part, out var n))
                    maxNum = Math.Max(maxNum, n);
            }

            var next = maxNum + 1;
            var formatted = prefix + next.ToString(new string('0', numLen));

            // Nếu vượt 7 ký tự thì fallback an toàn
            if (formatted.Length > 7)
                return "LSP" + next.ToString("D4");

            return formatted;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
