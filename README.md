# ⌚ Website Quản Lý & Bán Đồng Hồ

Website thương mại điện tử quản lý và kinh doanh đồng hồ được xây dựng bằng **ASP.NET MVC**, kết hợp **Entity Framework, SQL Server và ASP.NET Web API**.

Hệ thống được phát triển nhằm mô phỏng một website bán hàng trực tuyến với đầy đủ các chức năng dành cho **khách hàng** và **quản trị viên (Admin)**.

---

## 📖 Mục lục

- [Giới thiệu](#-giới-thiệu)
- [Mục tiêu](#-mục-tiêu)
- [Chức năng](#-chức-năng)
- [Công nghệ sử dụng](#️-công-nghệ-sử-dụng)
- [Kiến trúc hệ thống](#️-kiến-trúc-hệ-thống)
- [Cấu trúc project](#-cấu-trúc-project)
- [Cơ sở dữ liệu](#️-cơ-sở-dữ-liệu)
- [Web API](#-web-api)
- [Yêu cầu môi trường](#-yêu-cầu-môi-trường)
- [Cài đặt](#-cài-đặt)
- [Chạy project](#️-chạy-project)
- [Quy trình sử dụng](#-quy-trình-sử-dụng)
- [Giao diện](#-giao-diện)
- [Hướng phát triển](#-hướng-phát-triển)
- [Thành viên](#-thành-viên)
- [License](#-license)

---

## 📌 Giới thiệu

**Website Quản Lý & Bán Đồng Hồ** là một hệ thống thương mại điện tử cho phép người dùng tìm kiếm, xem thông tin và mua các sản phẩm đồng hồ trực tuyến.

Hệ thống được chia thành hai khu vực chính:

### 👤 Client

Khu vực dành cho khách hàng, cung cấp các chức năng:

- Xem sản phẩm
- Tìm kiếm sản phẩm
- Xem thông tin chi tiết
- Quản lý giỏ hàng
- Đặt hàng
- Quản lý tài khoản
- Xem lịch sử mua hàng
- Bình luận và đánh giá sản phẩm
- Theo dõi các chương trình khuyến mãi

### 🛠️ Admin

Khu vực quản trị cho phép quản lý toàn bộ dữ liệu của website:

- Sản phẩm
- Loại sản phẩm
- Thương hiệu
- Khách hàng
- Tài khoản
- Đơn hàng
- Khuyến mãi
- Bình luận
- Báo cáo

---

## 🎯 Mục tiêu

Project được xây dựng với các mục tiêu:

- Áp dụng kiến thức lập trình web vào một hệ thống thực tế.
- Làm quen với mô hình **ASP.NET MVC**.
- Sử dụng **Entity Framework** để thao tác với cơ sở dữ liệu.
- Xây dựng và sử dụng **ASP.NET Web API**.
- Làm việc với dữ liệu **JSON**.
- Xây dựng hệ thống đăng nhập và phân quyền.
- Xây dựng quy trình mua hàng trực tuyến.
- Quản lý dữ liệu thông qua khu vực Admin.
- Thực hành tổ chức một project web có nhiều module.

---

# 🚀 Chức năng

## 👤 1. Chức năng khách hàng

### 🔐 Tài khoản

- Đăng ký tài khoản.
- Đăng nhập.
- Đăng xuất.
- Quản lý thông tin cá nhân.
- Cập nhật thông tin tài khoản.
- Thay đổi mật khẩu.

### 🏠 Trang chủ

- Hiển thị sản phẩm.
- Hiển thị sản phẩm nổi bật.
- Hiển thị sản phẩm mới.
- Hiển thị thông tin khuyến mãi.
- Điều hướng đến các danh mục sản phẩm.

### 🔎 Tìm kiếm sản phẩm

Khách hàng có thể:

- Tìm kiếm theo tên sản phẩm.
- Xem danh sách sản phẩm.
- Lọc sản phẩm theo loại.
- Xem thông tin chi tiết sản phẩm.

### ⌚ Chi tiết sản phẩm

Thông tin sản phẩm bao gồm:

- Tên sản phẩm.
- Hình ảnh.
- Giá bán.
- Thương hiệu.
- Loại sản phẩm.
- Màu sắc.
- Kích thước.
- Số lượng.
- Thông tin mô tả.

### 🛒 Giỏ hàng

Khách hàng có thể:

- Thêm sản phẩm vào giỏ hàng.
- Thay đổi số lượng.
- Xóa sản phẩm.
- Xem tổng tiền.
- Kiểm tra danh sách sản phẩm trước khi đặt hàng.

### 📦 Đặt hàng

Quy trình đặt hàng:

```text
Xem sản phẩm
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
