# 📚 Arna Mini Learning Management System (LMS)

## 🌟 Nama Project
**Mini Learning Management System (LMS)**

## 📝 Deskripsi Project
Proyek ini adalah implementasi sederhana dari sebuah sistem manajemen pembelajaran online (Learning Management System - LMS) menggunakan teknologi .NET dan Blazor. Sistem ini memungkinkan pengguna untuk membuat, mengelola, dan menyelesaikan tugas dalam bentuk pilihan ganda (Multiple Choice Questions - MCQ). Fitur utama termasuk autentikasi pengguna, penilaian otomatis, dan pemantauan progress siswa.

## ⚙️ Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio atau Visual Studio Code](https://code.visualstudio.com/)
- Database PostgreSQL (untuk backend)

## 🏁 Getting Started

### 1. Clone Repository
```bash
git clone https://github.com/your-repo-url.git
cd Mini-LMS
```

### 2. Setup Backend
#### a. Install Dependencies
```bash
cd Backend
dotnet restore
```

#### b. Konfigurasi Database
- Pastikan Anda memiliki server PostgreSQL.
- Edit file `appsettings.json` di folder `Backend`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=mini_lms;Username=your_username;Password=your_password;"
  },
  "Jwt": {
    "Issuer": "MiniLMS",
    "Audience": "MiniLMS",
    "Key": "your_jwt_secret_key"
  }
}
```

#### c. Migrasi Database
```bash
dotnet ef database update
```

### 3. Setup Frontend
#### a. Install Dependencies
```bash
cd ../Frontend
npm install
```

#### b. Build dan Run
```bash
dotnet run --project Backend/Backend.csproj &
dotnet dev-certs https --trust
dotnet watch run --project Frontend/Frontend.csproj
```

### 4. Akses Aplikasi
Buka browser dan kunjungi:
- **Backend API**: `https://localhost:5083/swagger/index.html`
- **Frontend**: `https://localhost:5083`

## 👤 Roles & Features

### Role Pengguna
1. **Manager**
   - Bisa membuat, mengedit, menghapus, dan melihat semua tugas.
   - Melihat hasil pekerjaan siswa.
   
2. **Learner (Siswa)**
   - Menyelesaikan tugas dalam bentuk pilihan ganda.
   - Melihat hasil pekerjaannya setelah menyelesaikan tugas.

### Fitur Utama
- **Autentikasi**: Login dengan email dan password.
- **Pembuatan Tugas**: Membuat tugas baru dengan pertanyaan pilihan ganda.
- **Penilaian Otomatis**: Hasil pekerjaan siswa dinilai secara otomatis.
- **Review Jawaban**: Guru bisa mereview jawaban siswa secara detail.
- **Dashboard Siswa**: Melihat daftar tugas dan hasil pekerjaan.

## 📂 Struktur Folder

### Backend
```
Backend/
├── Controllers/
│   ├── AssignmentsController.cs
│   ├── AuthController.cs
│   └── LearnerController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── AssignmentReviewDto.cs
│   ├── AuthResponseDto.cs
│   ├── CreateAssignmentDto.cs
│   ├── GetAssignmentDto.cs
│   ├── LoginRequestDto.cs
│   ├── MCQDto.cs
│   ├── SubmissionResultDto.cs
│   └── UpdateAssignmentDto.cs
├── Models/
│   ├── Assignment.cs
│   ├── AssignmentProgress.cs
│   ├── McqQuestion.cs
│   ├── Role.cs
│   └── User.cs
├── Repositories/
│   ├── AssignmentRepository.cs
│   ├── IUserRepository.cs
│   └── UserRepository.cs
├── Services/
│   ├── AssignmentService.cs
│   ├── AuthService.cs
│   └── LearnerService.cs
└── Program.cs
```

### Frontend
```
Frontend/
├── Components/
│   ├── AssignmentItem.razor
│   ├── AuthRouteGuard.razor
│   └── MCQItem.razor
├── DTOs/
│   ├── AssignmentDto.cs
│   ├── AssignmentReviewDto.cs
│   ├── LoginResponseDto.cs
│   ├── MCQDto.cs
│   ├── QuestionAnswerDto.cs
│   ├── SubmissionDto.cs
│   └── SubmissionResultDto.cs
├── Layout/
│   ├── MainLayout.razor
│   ├── MainLayout.razor.css
│   ├── NavMenu.razor
│   └── NavMenu.razor.css
├── Pages/
│   ├── AssignmentDetail.razor
│   ├── Assignments.razor
│   ├── Counter.razor
│   ├── CreateAssignments.razor
│   ├── EditAssignment.razor
│   ├── Home.razor
│   ├── Login.razor
│   ├── Review.razor
│   ├── Tugas.razor
│   ├── TugasDetail.razor
│   └── Unauthorized.razor
├── Services/
│   ├── AssignmentService.cs
│   ├── AuthService.cs
│   ├── AuthStateProvider.cs
│   ├── LearnerAssignmentService.cs
│   └── LocalStorageService.cs
└── wwwroot/
```

## 💾 Database Schema

### Tabel Utama
1. **Users**
   - `Id`: Primary Key
   - `Email`: Email pengguna
   - `Password`: Password hash
   - `RoleId`: Foreign Key ke tabel `Roles`

2. **Roles**
   - `Id`: Primary Key
   - `Name`: Nama role (e.g., "Manager", "Learner")

3. **Assignments**
   - `Id`: Primary Key
   - `Title`: Judul tugas
   - `Description`: Deskripsi tugas
   - `Media`: Link media (e.g., video tutorial)
   - `IsActive`: Status aktif/tidak aktif
   - `CreatedAt`: Waktu pembuatan

4. **McqQuestions**
   - `Id`: Primary Key
   - `QuestionText`: Pertanyaan
   - `OptionA`, `OptionB`, `OptionC`, `OptionD`: Pilihan jawaban
   - `CorrectAnswer`: Jawaban benar (A/B/C/D)
   - `AssignmentId`: Foreign Key ke tabel `Assignments`

5. **AssignmentProgress**
   - `Id`: Primary Key
   - `UserId`: Foreign Key ke tabel `Users`
   - `AssignmentId`: Foreign Key ke tabel `Assignments`
   - `Score`: Nilai yang diperoleh
   - `SubmittedAt`: Waktu pengumpulan

6. **SubmittedAnswers**
   - `Id`: Primary Key
   - `AssignmentProgressId`: Foreign Key ke tabel `AssignmentProgress`
   - `QuestionId`: Foreign Key ke tabel `McqQuestions`
   - `GivenAnswer`: Jawaban yang diberikan
   - `IsCorrect`: Apakah jawaban benar?

## 🌐 API Endpoints

### 1. Authentication
- **POST `/api/auth/login`**
  - Deskripsi: Login pengguna.
  - Request Body:
    ```json
    {
      "email": "user@example.com",
      "password": "password123"
    }
    ```
  - Response:
    ```json
    {
      "userId": "user1",
      "username": "John Doe",
      "role": "Manager",
      "token": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
    ```

### 2. Assignments
- **GET `/api/assignments`**
  - Deskripsi: Mendapatkan daftar tugas.
- **GET `/api/assignments/{id}`**
  - Deskripsi: Mendapatkan detail tugas tanpa soal.
- **GET `/api/assignments/{id}/with-questions`**
  - Deskripsi: Mendapatkan detail tugas beserta soal-soalnya.
- **POST `/api/assignments`**
  - Deskripsi: Membuat tugas baru.
- **PUT `/api/assignments/{id}`**
  - Deskripsi: Mengedit tugas.
- **DELETE `/api/assignments/{id}`**
  - Deskripsi: Menghapus tugas.
- **GET `/api/assignments/{assignmentId}/submissions`**
  - Deskripsi: Mendapatkan hasil pekerjaan siswa untuk suatu tugas.
- **GET `/api/assignments/review/{assignmentId}/{userId}`**
  - Deskripsi: Mendapatkan review jawaban siswa untuk suatu tugas.

### 3. Learner Submissions
- **POST `/api/Learner/submit`**
  - Deskripsi: Mengirimkan jawaban siswa.
  - Request Body:
    ```json
    {
      "assignmentId": 1,
      "answers": {
        "1": "A",
        "2": "B"
      }
    }
    ```

## 🧑‍💻 User Dummy

### 1. Manager
- **Email**: `manager@example.com`
- **Password**: `password123`
- **Role**: `Manager`

### 2. Learner
- **Email**: `learner@example.com`
- **Password**: `password123`
- **Role**: `Learner`

## 🛠️ How to Contribute
1. Fork repository ini.
2. Buat branch baru: `git checkout -b feature/new-feature`.
3. Commit perubahan: `git commit -m "Add new feature"`.
4. Push ke branch Anda: `git push origin feature/new-feature`.
5. Buat pull request.
