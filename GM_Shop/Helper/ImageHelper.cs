namespace GM_Shop.Helper
{
    public class ImageHelper
    {
        public static string UpLoadImage(IFormFile Hinh, string folder)
        {
            try
            {
                // Kiểm tra file null
                if (Hinh == null || Hinh.Length == 0)
                    return string.Empty;

                // Tạo tên file an toàn
                var extension = Path.GetExtension(Hinh.FileName); // Lấy phần mở rộng
                var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{extension}";

                // Đường dẫn lưu file
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folder);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fullPath = Path.Combine(folderPath, fileName);

                // Lưu file
                using (var myFile = new FileStream(fullPath, FileMode.Create))
                {
                    Hinh.CopyTo(myFile);
                }

                return fileName; // Trả về tên file
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine($"Error uploading image: {ex}");
                return string.Empty;
            }
        }
    }
}
