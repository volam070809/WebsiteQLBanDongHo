API JSON (MVC) - LOAI SAN PHAM

1) RUN project (F5)
2) Test endpoints:

GET    http://localhost:<port>/api/loai
GET    http://localhost:<port>/api/loai/LSP0001
POST   http://localhost:<port>/api/loai
       Body JSON: { "tenLoai": "Dong ho nam" }
PUT    http://localhost:<port>/api/loai/LSP0001
       Body JSON: { "tenLoai": "Update" }
DELETE http://localhost:<port>/api/loai/LSP0001

Note:
- This API does NOT use ASP.NET WebApi, so you won't get the System.Net.Http.Formatting / bindingRedirect headaches.
- If DELETE fails, it's usually because FK constraints (products use that category) => returns 409.
