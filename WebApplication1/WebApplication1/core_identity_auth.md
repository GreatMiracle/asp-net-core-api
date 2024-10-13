# Xác thực và Quản lý Người dùng trong ASP.NET Core Identity

Trong quy trình xác thực và quản lý người dùng của ASP.NET Core Identity, các bảng mặc định liên quan đến xác thực và phân quyền người dùng sẽ được Entity Framework tạo ra khi chạy migration. Dưới đây là một số bảng quan trọng mà bạn cần biết:

## 1. AspNetUsers
**Chức năng:** Lưu trữ thông tin người dùng (user).  
**Các cột quan trọng:**
- **Id:** ID duy nhất của người dùng (GUID).
- **UserName:** Tên đăng nhập.
- **NormalizedUserName:** Tên đăng nhập được chuẩn hóa (viết hoa).
- **Email:** Địa chỉ email.
- **PasswordHash:** Hash của mật khẩu.
- **PhoneNumber:** Số điện thoại.
- **ConcurrencyStamp:** Dùng để theo dõi sự thay đổi của các đối tượng.

**Ví dụ về bảng AspNetUsers:**

| Id                 | UserName   | Email                | PhoneNumber     |
|--------------------|------------|----------------------|------------------|
| 1a2b3c4d-...       | user1      | user1@example.com    | 0123456789       |
| 2b3c4d5e-...       | user2      | user2@example.com    | 0987654321       |
| 3c4d5e6f-...       | admin      | admin@example.com    | 0112233445       |

## 2. AspNetRoles
**Chức năng:** Lưu trữ thông tin về các vai trò (roles) của hệ thống.  
**Các cột quan trọng:**
- **Id:** ID của vai trò (role), kiểu GUID.
- **Name:** Tên vai trò, ví dụ Admin, User, Reader, Writer.
- **NormalizedName:** Tên vai trò được chuẩn hóa (viết hoa), ví dụ ADMIN, USER.
- **ConcurrencyStamp:** Concurrency token để theo dõi các thay đổi.

**Ví dụ về bảng AspNetRoles:**

| Id                 | Name   | NormalizedName | ConcurrencyStamp     |
|--------------------|--------|-----------------|-----------------------|
| fbc2b73f-...       | Reader | READER          | fbc2b73f-...          |
| b871c2ef-...       | Writer | WRITER          | b871c2ef-...          |
| c982d3fe-...       | Admin  | ADMIN           | c982d3fe-...          |

## 3. AspNetUserRoles
**Chức năng:** Liên kết giữa người dùng (users) và vai trò (roles), tức là một người dùng có thể có nhiều vai trò.  
**Các cột quan trọng:**
- **UserId:** ID của người dùng, liên kết với bảng AspNetUsers.
- **RoleId:** ID của vai trò, liên kết với bảng AspNetRoles.

**Ví dụ về bảng AspNetUserRoles:**

| UserId            | RoleId           |
|--------------------|------------------|
| 1a2b3c4d-...       | fbc2b73f-...     |
| 2b3c4d5e-...       | b871c2ef-...     |
| 3c4d5e6f-...       | c982d3fe-...     |

## 4. AspNetUserClaims
**Chức năng:** Lưu trữ các claims của người dùng, đại diện cho những quyền truy cập hoặc thông tin xác thực của người dùng.  
**Các cột quan trọng:**
- **Id:** ID của claim.
- **UserId:** ID của người dùng.
- **ClaimType:** Loại claim (ví dụ Email, Role).
- **ClaimValue:** Giá trị của claim (ví dụ admin@example.com).

**Ví dụ về bảng AspNetUserClaims:**

| Id   | UserId            | ClaimType | ClaimValue               |
|------|--------------------|-----------|--------------------------|
| 1    | 1a2b3c4d-...       | Email     | user1@example.com        |
| 2    | 2b3c4d5e-...       | Role      | Reader                   |
| 3    | 3c4d5e6f-...       | Role      | Admin                    |

## 5. AspNetRoleClaims
**Chức năng:** Lưu trữ các claims liên quan đến vai trò.  
**Các cột quan trọng:**
- **Id:** ID của claim.
- **RoleId:** ID của vai trò (role).
- **ClaimType:** Loại claim.
- **ClaimValue:** Giá trị claim.

**Ví dụ về bảng AspNetRoleClaims:**

| Id   | RoleId           | ClaimType | ClaimValue               |
|------|-------------------|-----------|--------------------------|
| 1    | fbc2b73f-...      | Permission| CanRead                  |
| 2    | b871c2ef-...      | Permission| CanWrite                 |
| 3    | c982d3fe-...      | Permission| CanManageUsers           |

## 6. AspNetUserLogins
**Chức năng:** Lưu thông tin về việc đăng nhập của người dùng thông qua các provider bên ngoài (ví dụ Google, Facebook).  
**Các cột quan trọng:**
- **LoginProvider:** Provider đăng nhập (Google, Facebook, v.v.).
- **ProviderKey:** Khóa duy nhất do provider cung cấp.
- **UserId:** ID của người dùng.

**Ví dụ về bảng AspNetUserLogins:**

| LoginProvider | ProviderKey         | UserId            |
|---------------|---------------------|--------------------|
| Google        | google_user1_key    | 1a2b3c4d-...       |
| Facebook      | facebook_user2_key  | 2b3c4d5e-...       |
| Google        | google_admin_key     | 3c4d5e6f-...       |

## 7. AspNetUserTokens
**Chức năng:** Lưu trữ các token được cấp cho người dùng (ví dụ token xác thực, token khôi phục mật khẩu).  
**Các cột quan trọng:**
- **UserId:** ID của người dùng.
- **LoginProvider:** Provider cung cấp token.
- **Name:** Tên của token.
- **Value:** Giá trị của token.

**Ví dụ về bảng AspNetUserTokens:**

| UserId            | LoginProvider | Name              | Value                  |
|--------------------|---------------|-------------------|------------------------|
| 1a2b3c4d-...       | Default       | AuthToken         | token_value_1          |
| 2b3c4d5e-...       | Default       | RefreshToken      | token_value_2          |
| 3c4d5e6f-...       | Default       | PasswordResetToken | token_value_3          |

## Cách seed dữ liệu vào bảng AspNetRoles
Khi seed dữ liệu cho bảng AspNetRoles, bạn sẽ thêm các vai trò (roles) như sau:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // Seed các vai trò (roles)
    var roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Reader",
            NormalizedName = "READER",
            ConcurrencyStamp = Guid.NewGuid().ToString()
        },
        new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Writer",
            NormalizedName = "WRITER",
            ConcurrencyStamp = Guid.NewGuid().ToString()
        }
    };

    builder.Entity<IdentityRole>().HasData(roles);
}