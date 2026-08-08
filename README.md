# ⌚ Website Quản Lý & Bán Đồng Hồ

Website thương mại điện tử quản lý và kinh doanh đồng hồ được xây dựng bằng **ASP.NET MVC**, sử dụng **Entity Framework**, **SQL Server** và tích hợp **ASP.NET Web API** để cung cấp dữ liệu dưới dạng JSON.

Project gồm hai khu vực chính:

- **Website khách hàng (Client):** xem sản phẩm, tìm kiếm, giỏ hàng, đặt hàng, tài khoản và bình luận.
- **Khu vực quản trị (Admin):** quản lý sản phẩm, loại sản phẩm, thương hiệu, khách hàng, tài khoản, đơn hàng, khuyến mãi, bình luận và báo cáo.

---

<a id="muc-luc"></a>

## 📖 Mục lục

- [Giới thiệu](#gioi-thieu)
- [Mục tiêu](#muc-tieu)
- [Chức năng](#chuc-nang)
- [Công nghệ sử dụng](#cong-nghe-su-dung)
- [Kiến trúc hệ thống](#kien-truc-he-thong)
- [Cấu trúc project](#cau-truc-project)
- [Cơ sở dữ liệu](#co-so-du-lieu)
- [Web API](#web-api)
- [Yêu cầu môi trường](#yeu-cau-moi-truong)
- [Cài đặt](#cai-dat)
- [Cấu hình Database](#cau-hinh-database)
- [Chạy project](#chay-project)
- [Quy trình mua hàng](#quy-trinh-mua-hang)
- [Khu vực Admin](#khu-vuc-admin)
- [Kiểm thử](#kiem-thu)
- [Hướng phát triển](#huong-phat-trien)
- [Thành viên](#thanh-vien)
- [License](#license)

---

<a id="gioi-thieu"></a>

## 📌 Giới thiệu

**Website Quản Lý & Bán Đồng Hồ** là một hệ thống thương mại điện tử được xây dựng để mô phỏng quy trình kinh doanh đồng hồ trực tuyến.

Hệ thống cho phép khách hàng tìm kiếm và xem thông tin sản phẩm, quản lý giỏ hàng, đặt hàng và theo dõi đơn hàng. Đồng thời, Admin có thể quản lý dữ liệu sản phẩm, khách hàng, đơn hàng và các nội dung liên quan đến hoạt động kinh doanh.

Project được phát triển bằng kiến trúc **ASP.NET MVC**, trong đó:

- **Model** chịu trách nhiệm quản lý dữ liệu và nghiệp vụ.
- **View** hiển thị giao diện cho người dùng.
- **Controller** tiếp nhận request và xử lý luồng nghiệp vụ.
- **Entity Framework** hỗ trợ kết nối và thao tác với SQL Server.
- **ASP.NET Web API** cung cấp dữ liệu JSON cho API.

---

<a id="muc-tieu"></a>

## 🎯 Mục tiêu

Project được thực hiện nhằm:

- Áp dụng kiến thức lập trình web vào một hệ thống thực tế.
- Thực hành xây dựng ứng dụng bằng **ASP.NET MVC**.
- Sử dụng **Entity Framework** trong việc truy xuất và quản lý dữ liệu.
- Làm việc với **SQL Server**.
- Xây dựng **ASP.NET Web API**.
- Trao đổi dữ liệu thông qua **JSON**.
- Xây dựng chức năng đăng nhập và quản lý tài khoản.
- Xây dựng quy trình giỏ hàng và đặt hàng.
- Thực hành xây dựng khu vực quản trị.
- Tổ chức source code thành các Controller, Model, View và Service tương ứng.

---

<a id="chuc-nang"></a>

## 🚀 Chức năng

### 👤 Chức năng khách hàng

#### Tài khoản

- Đăng ký tài khoản.
- Đăng nhập.
- Đăng xuất.
- Xem và cập nhật thông tin cá nhân.
- Quản lý thông tin tài khoản.

#### Sản phẩm

- Xem danh sách sản phẩm.
- Xem chi tiết sản phẩm.
- Tìm kiếm sản phẩm.
- Xem sản phẩm theo loại.
- Xem thông tin thương hiệu.
- Xem hình ảnh sản phẩm.
- Xem giá và thông tin chi tiết.

#### Giỏ hàng

- Thêm sản phẩm vào giỏ hàng.
- Cập nhật số lượng.
- Xóa sản phẩm khỏi giỏ hàng.
- Xem tổng tiền.
- Kiểm tra thông tin trước khi đặt hàng.

#### Đặt hàng

Khách hàng có thể thực hiện quy trình:

```text
Xem sản phẩm
      ↓
Xem chi tiết
      ↓
Thêm vào giỏ hàng
      ↓
Kiểm tra giỏ hàng
      ↓
Nhập thông tin đặt hàng
      ↓
Xác nhận đơn hàng
      ↓
Tạo đơn hàng
```

#### Đơn hàng

- Xem danh sách đơn hàng.
- Xem chi tiết đơn hàng.
- Xem các sản phẩm trong đơn hàng.
- Theo dõi thông tin đơn hàng.

#### Bình luận

- Xem bình luận về sản phẩm.
- Gửi bình luận.
- Quản lý nội dung bình luận theo quyền của người dùng.

#### Khuyến mãi

- Xem thông tin chương trình khuyến mãi.
- Xem các sản phẩm áp dụng khuyến mãi.

---

### 🛠️ Chức năng Admin

Khu vực Admin được tổ chức trong thư mục:

```text
Areas/Admin/
```

#### Dashboard

- Hiển thị tổng quan hệ thống.
- Theo dõi các thông tin quản trị.
- Truy cập nhanh đến các chức năng quản lý.

#### Quản lý sản phẩm

- Xem danh sách sản phẩm.
- Thêm sản phẩm.
- Cập nhật sản phẩm.
- Xem chi tiết sản phẩm.
- Xóa sản phẩm.

#### Quản lý loại sản phẩm

- Xem danh sách loại sản phẩm.
- Thêm loại sản phẩm.
- Cập nhật loại sản phẩm.
- Xóa loại sản phẩm.

#### Quản lý thương hiệu

- Xem danh sách thương hiệu.
- Thêm thương hiệu.
- Cập nhật thương hiệu.
- Xóa thương hiệu.

#### Quản lý khách hàng

- Xem danh sách khách hàng.
- Xem thông tin khách hàng.
- Quản lý dữ liệu khách hàng.

#### Quản lý tài khoản

- Xem danh sách tài khoản.
- Cập nhật tài khoản.
- Thay đổi thông tin tài khoản.
- Đặt lại mật khẩu.

#### Quản lý đơn hàng

- Xem danh sách đơn hàng.
- Xem chi tiết đơn hàng.
- Tạo và quản lý đơn hàng.
- Xử lý thông tin đơn hàng.

#### Quản lý khuyến mãi

- Quản lý chương trình khuyến mãi.
- Thêm khuyến mãi.
- Cập nhật khuyến mãi.
- Xóa khuyến mãi.
- Quản lý chi tiết khuyến mãi.

#### Quản lý bình luận

- Xem danh sách bình luận.
- Quản lý bình luận của khách hàng.

#### Báo cáo

Project có module báo cáo sản phẩm đã bán, được triển khai trong:

```text
Areas/Admin/Controllers/ReportController.cs
```

---

<a id="cong-nghe-su-dung"></a>

## 🛠️ Công nghệ sử dụng

| Công nghệ | Vai trò |
|---|---|
| C# | Ngôn ngữ lập trình chính |
| ASP.NET MVC | Xây dựng website |
| ASP.NET Web API | Xây dựng API |
| Entity Framework | ORM và truy xuất dữ liệu |
| SQL Server | Hệ quản trị cơ sở dữ liệu |
| HTML5 | Xây dựng cấu trúc giao diện |
| CSS3 | Thiết kế giao diện |
| JavaScript | Xử lý phía client |
| jQuery | Tương tác với giao diện và request |
| Bootstrap | Hỗ trợ giao diện |
| JSON | Định dạng trao đổi dữ liệu |
| Visual Studio | Môi trường phát triển |

---

<a id="kien-truc-he-thong"></a>

## 🏗️ Kiến trúc hệ thống

Project sử dụng mô hình **ASP.NET MVC**:

```text
                 ┌──────────────────┐
                 │      Client      │
                 │     Browser      │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │    Controller    │
                 └────────┬─────────┘
                          │
                ┌─────────┴─────────┐
                ▼                   ▼
        ┌──────────────┐    ┌──────────────┐
        │    Model     │    │     View     │
        └──────┬───────┘    └──────────────┘
               │
               ▼
        ┌──────────────┐
        │    Service   │
        └──────┬───────┘
               │
               ▼
        ┌──────────────┐
        │    Entity    │
        │   Framework  │
        └──────┬───────┘
               │
               ▼
        ┌──────────────┐
        │  SQL Server  │
        └──────────────┘
```

Ngoài MVC, project có Web API:

```text
Client
  │
  ▼
ASP.NET Web API
  │
  ▼
Controller
  │
  ▼
JSON Response
```

---

<a id="cau-truc-project"></a>

## 📂 Cấu trúc project

```text
WebsiteQLBanDongHo_SIMPLE_JSON_API_LOAI_small/
│
├── Areas/
│   └── Admin/
│       ├── Controllers/
│       ├── Models/
│       ├── Views/
│       └── Content/
│
├── App_Start/
│   ├── BundleConfig.cs
│   ├── FilterConfig.cs
│   ├── RouteConfig.cs
│   └── WebApiConfig.cs
│
├── Common/
│   ├── CommonConstands.cs
│   ├── ConfigHelper.cs
│   ├── Encryptor.cs
│   ├── MailHelper.cs
│   └── UserLogin.cs
│
├── Controllers/
│   ├── AccountController.cs
│   ├── CartController.cs
│   ├── CusInfoController.cs
│   ├── HomeController.cs
│   ├── OrderController.cs
│   ├── ProductController.cs
│   └── Api/
│       └── LoaiApiController.cs
│
├── Models/
│
├── Views/
│
├── WebsiteQLBanDongHoDomain/
│
├── Content/
│
├── Scripts/
│
├── images/
│
├── Global.asax
├── Web.config
├── packages.config
├── SQL_Fix_GioiTinh.sql
└── WebsiteQLBanDongHo.csproj
```

> Các thư mục `bin/`, `obj/` và `.vs/` là thư mục sinh ra trong quá trình build hoặc làm việc với Visual Studio và không nên đưa lên GitHub.

---

<a id="co-so-du-lieu"></a>

## 🗄️ Cơ sở dữ liệu

Project sử dụng **SQL Server** để lưu trữ dữ liệu.

Các nhóm dữ liệu chính của hệ thống gồm:

```text
Sản phẩm
   ├── Loại sản phẩm
   └── Thương hiệu

Khách hàng
   └── Tài khoản

Đơn hàng
   └── Chi tiết đơn hàng
          └── Sản phẩm

Khuyến mãi
   └── Chi tiết khuyến mãi

Sản phẩm
   └── Bình luận
```

Project có sử dụng **Entity Framework** để ánh xạ dữ liệu giữa database và các model/class trong ứng dụng.

Ngoài ra, repository có file:

```text
SQL_Fix_GioiTinh.sql
```

để hỗ trợ xử lý dữ liệu liên quan đến trường `GioiTinh`.

---

<a id="web-api"></a>

## 🔌 Web API

Project tích hợp **ASP.NET Web API**.

Controller API:

```text
Controllers/Api/LoaiApiController.cs
```

API được xây dựng để xử lý dữ liệu **Loại sản phẩm** và trả về dữ liệu theo định dạng JSON.

### Endpoint

```text
/api/LoaiApi
```

API có thể được sử dụng để lấy dữ liệu loại sản phẩm từ hệ thống.

> Endpoint cụ thể phụ thuộc vào cấu hình route trong `App_Start/WebApiConfig.cs`.

---

<a id="yeu-cau-moi-truong"></a>

## 💻 Yêu cầu môi trường

Để chạy project cần chuẩn bị:

- Windows.
- Visual Studio 2019 hoặc Visual Studio 2022.
- .NET Framework tương ứng với project.
- SQL Server.
- SQL Server Management Studio (SSMS).
- NuGet Package Manager.

---

<a id="cai-dat"></a>

## 📥 Cài đặt

### 1. Clone repository

```bash
git clone https://github.com/volam070809/WebsiteQLBanDongHo.git
```

### 2. Di chuyển vào thư mục project

```bash
cd WebsiteQLBanDongHo
```

### 3. Mở Solution

Mở file:

```text
WebsiteQLBanDongHo.sln
```

bằng Visual Studio.

### 4. Restore NuGet Packages

Trong Visual Studio, thực hiện restore NuGet Packages.

Sau đó:

```text
Build
→ Rebuild Solution
```

để kiểm tra project có build thành công hay không.

---

<a id="cau-hinh-database"></a>

## 🗃️ Cấu hình Database

Mở file:

```text
Web.config
```

Kiểm tra phần:

```xml
<connectionStrings>
```

Cập nhật connection string để phù hợp với SQL Server trên máy của bạn.

Ví dụ:

```xml
<connectionStrings>
    <add name="DefaultConnection"
         connectionString="..."
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

> Không nên commit password hoặc thông tin đăng nhập database thật lên GitHub.

Sau khi cấu hình database, kiểm tra lại Entity Framework và build project.

---

<a id="chay-project"></a>

## ▶️ Chạy project

Sau khi hoàn thành cấu hình:

1. Mở `WebsiteQLBanDongHo.sln`.
2. Kiểm tra connection string.
3. Restore NuGet Packages.
4. Build Solution.
5. Chọn project chính làm **Startup Project**.
6. Nhấn `F5` hoặc `Ctrl + F5`.

Website sẽ được mở trên trình duyệt thông qua IIS Express hoặc web server được cấu hình trong Visual Studio.

---

<a id="quy-trinh-mua-hang"></a>

## 🛒 Quy trình mua hàng

```text
             ┌──────────────┐
             │   Trang chủ  │
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │ Xem sản phẩm │
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │ Chi tiết SP  │
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │ Thêm giỏ hàng│
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │   Giỏ hàng   │
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │   Đặt hàng   │
             └──────┬───────┘
                    ↓
             ┌──────────────┐
             │ Tạo đơn hàng │
             └──────────────┘
```

---

<a id="khu-vuc-admin"></a>

## 🛠️ Khu vực Admin

Khu vực Admin nằm trong:

```text
Areas/Admin/
```

Các module chính:

```text
Admin
├── Dashboard
├── Account
├── Customer
├── User
├── Product
├── ProductBrand
├── ProductCategory
├── Order
├── Promotion
├── PromotionDetail
├── Comment
└── Report
```

Mỗi module được tổ chức thành các thành phần riêng như:

```text
Controller
Model / Service / ViewModel
View
```

giúp tách biệt phần xử lý nghiệp vụ và giao diện quản trị.

---

<a id="kiem-thu"></a>

## 🧪 Kiểm thử

Các chức năng chính cần kiểm tra:

### Khách hàng

- [ ] Đăng ký.
- [ ] Đăng nhập.
- [ ] Đăng xuất.
- [ ] Xem sản phẩm.
- [ ] Tìm kiếm sản phẩm.
- [ ] Xem chi tiết sản phẩm.
- [ ] Thêm sản phẩm vào giỏ hàng.
- [ ] Cập nhật giỏ hàng.
- [ ] Xóa sản phẩm khỏi giỏ hàng.
- [ ] Đặt hàng.
- [ ] Xem đơn hàng.
- [ ] Cập nhật thông tin cá nhân.
- [ ] Bình luận sản phẩm.

### Admin

- [ ] Đăng nhập Admin.
- [ ] Xem Dashboard.
- [ ] Quản lý sản phẩm.
- [ ] Quản lý loại sản phẩm.
- [ ] Quản lý thương hiệu.
- [ ] Quản lý khách hàng.
- [ ] Quản lý tài khoản.
- [ ] Quản lý đơn hàng.
- [ ] Quản lý khuyến mãi.
- [ ] Quản lý bình luận.
- [ ] Xem báo cáo.

### API

- [ ] Kiểm tra route API.
- [ ] Kiểm tra request.
- [ ] Kiểm tra JSON response.
- [ ] Kiểm tra dữ liệu loại sản phẩm.

---

<a id="huong-phat-trien"></a>

## 🔮 Hướng phát triển

Trong các phiên bản tiếp theo có thể mở rộng:

- Tích hợp thanh toán trực tuyến.
- Tích hợp VNPay hoặc MoMo.
- Gửi email xác nhận đơn hàng.
- Quên mật khẩu qua email.
- Xác thực OTP.
- Theo dõi trạng thái giao hàng.
- Cải thiện bảo mật.
- Phân quyền Admin/User chi tiết hơn.
- Tối ưu truy vấn SQL.
- Tối ưu tốc độ website.
- Cải thiện giao diện responsive trên mobile.
- Bổ sung biểu đồ thống kê doanh thu.
- Mở rộng Web API thành RESTful API hoàn chỉnh.
- Xây dựng ứng dụng mobile sử dụng API.

---

<a id="thanh-vien"></a>

## 👥 Thành viên

**Nhóm 10 – Lớp 14DHTH13**

Project được thực hiện với mục đích học tập và nghiên cứu trong môn Lập trình Web.

---

<a id="license"></a>

## 📄 License

This project is developed for educational purposes.

Copyright © 2026 Group 10.
