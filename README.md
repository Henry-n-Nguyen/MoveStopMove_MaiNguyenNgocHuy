# 🟢 MoveStopMove

**MoveStopMove** là một dự án game 3D thuộc thể loại **Hyper-Casual** được xây dựng bằng Unity Engine. Người chơi vào vai một người tham gia chiến đấu để thu thập chiến lợi phẩm.

---

## 🎮 Tổng quan trò chơi
Người chơi phải điều khiển nhân vật chạy liên tục, né tránh các đòn tấn công, tiêu diệt kẻ địch và thu thập vật phẩm để sinh tồn lâu nhất có thể.

* **Cơ chế tăng trưởng (Scaling System):** Nhân vật tự động thay đổi kích thước và tầm nhìn (Camera Zoom) dựa trên số lượng điểm (orbs) thu thập được.
* **Hệ thống AI Bots:** Các đối thủ máy có hành vi tự động tìm kiếm thức ăn và né tránh người chơi lớn hơn (Simple Steering Behaviors).
* **Vùng an toàn/Giới hạn:** Bản đồ có biên giới, buộc người chơi phải tương tác với nhau trong không gian hẹp dần.
* **Hệ thống vật phẩm (Power-ups):** Thu thập tiền vàng để nâng cấp điểm số và các Buff (Khiên bảo vệ, Nam châm hút tiền).
* **High Score:** Lưu trữ và hiển thị điểm số cao nhất và tài nguyên đang có một cách cục bộ bằng `PlayerPrefs`.

---

## 🛠 Kỹ thuật & Tư duy lập trình (Tech Stack)
Dự án này tập trung vào việc tối ưu hóa hiệu suất và cấu trúc mã nguồn sạch (Clean Code), phù hợp với tiêu chuẩn phát triển game chuyên nghiệp:

1.  **Object Pooling:** Sử dụng để quản lý các chướng ngại vật và hiệu ứng hạt (particles), giúp giảm thiểu việc `Instantiate` và `Destroy` liên tục, tránh gây giật lag do Garbage Collector.
2.  **State Machine:** Quản lý các trạng thái của nhân vật (Idle, Run, Jump, Slide, Death) một cách logic, dễ dàng mở rộng thêm các hành động mới.
3.  **ScriptableObjects:** Dùng để lưu trữ dữ liệu về chỉ số nhân vật và cấu hình vật phẩm, giúp tách biệt dữ liệu khỏi logic code.
4.  **Singleton Pattern:** Áp dụng cho Game Manager và UI Manager để quản lý vòng đời game một cách tập trung.
5.  **Observer Pattenrn:** Áp dụng các sự kiện được đăng kí và quan sát giúp quản lý luồng game. 
