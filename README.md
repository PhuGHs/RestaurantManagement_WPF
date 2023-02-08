# RestaurantManagement_WPF
Ứng dụng quản lý nhà hàng - Đầy đủ tính năng, mạnh mẽ và chính xác.
# Hướng dẫn sử dụng
1. Cài đặt các phần mềm cần thiết:
- Visual Studio
- SQL Server 2022 và SSMS
2. Tải source code về máy:
- Cách 1: Clone code bằng: 
https://github.com/PhuGHs/RestaurantManagement_WPF.git
- Cách 2: Tải file zip:

![image](https://user-images.githubusercontent.com/96371073/217535668-912332a7-3fc3-44ce-bcc3-7ef9bd38831b.png)

3. Tạo database:
- Trong thư mục vừa giải nén, tìm file db.sql, nhấn chuột phải và chọn Open with SSMS
- Cửa sổ SSMS xuất hiện, bôi đen toàn bộ script và nhấn F5 (hoặc bấm nút 'Execute')
- Lấy tên server:

![image](https://user-images.githubusercontent.com/96371073/217536513-997a445b-0c7c-4743-9571-53b8896dca2c.png)

- Cửa sổ mới xuất hiện, copy tên server của máy:

![image](https://user-images.githubusercontent.com/96371073/217536687-1558e695-5d34-4350-b416-1ed285515222.png)

- Quay lại thư mục đã giải nén lúc đầu, tìm đến file App.config trong Solution Explorer, thay dòng màu đỏ bằng tên server vừa copy:

![image](https://user-images.githubusercontent.com/96371073/217537262-f229cd56-1896-4d2e-a5b2-94d298bae8cf.png)

- Nhấn Ctrl + F5 để chạy chương trình
- Đăng nhập lần đầu bằng tài khoản: admin và mật khẩu: admin
- Sau đó vào tab 'Nhân viên', thêm mới nhân viên cùng tài khoản để có thể đăng nhập bằng quyền nhân viên.
