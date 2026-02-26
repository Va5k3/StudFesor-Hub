using BusinessLayer.DTOs;
using BusinessLayer.Implementations;
using DAL.Abstraction;
using Entities;
using Moq;
using Xunit;

namespace UnitTestt
{
    // =====================================================================
    // UserBusiness Tests
    // =====================================================================
    public class UserBusinessTests
    {
        private readonly Mock<IRepositoryUser> _userRepoMock;
        private readonly UserBusiness _userBusiness;

        public UserBusinessTests()
        {
            _userRepoMock = new Mock<IRepositoryUser>();
            _userBusiness = new UserBusiness(_userRepoMock.Object);
        }

        #region GetById

        [Fact]
        public void GetById_PostojeciStudent_VracaUserDTOSaRolomStudent()
        {
            // Arrange
            var user = new User
            {
                IdUser = 1,
                IdRole = 3,
                FirstName = "Ana",
                LastName = "Anić",
                Email = "ana@test.com"
            };

            _userRepoMock.Setup(x => x.Get(1)).Returns(user);

            // Act
            var result = _userBusiness.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Ana", result.FirstName);
            Assert.Equal("Anić", result.LastName);
            Assert.Equal("ana@test.com", result.Email);
            Assert.Equal("Student", result.RoleName);
        }

        [Fact]
        public void GetById_PostojeciProfesor_VracaUserDTOSaRolomProfesor()
        {
            // Arrange
            var user = new User
            {
                IdUser = 2,
                IdRole = 2,
                FirstName = "Marko",
                LastName = "Marković",
                Email = "marko@test.com"
            };

            _userRepoMock.Setup(x => x.Get(2)).Returns(user);

            // Act
            var result = _userBusiness.GetById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Profesor", result.RoleName);
        }

        [Fact]
        public void GetById_NepostojeciKorisnik_VracaNull()
        {
            // Arrange
            _userRepoMock.Setup(x => x.Get(999)).Returns((User?)null);

            // Act
            var result = _userBusiness.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetById_KorisnikSaNullImenom_VracaPrazanString()
        {
            // Arrange
            var user = new User
            {
                IdUser = 3,
                IdRole = 3,
                FirstName = null,
                LastName = null,
                Email = null
            };

            _userRepoMock.Setup(x => x.Get(3)).Returns(user);

            // Act
            var result = _userBusiness.GetById(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.FirstName);
            Assert.Equal(string.Empty, result.LastName);
            Assert.Equal(string.Empty, result.Email);
        }

        [Fact]
        public void GetById_ProveriDaLiJeRepositoryMetodaPozvana()
        {
            // Arrange
            _userRepoMock.Setup(x => x.Get(1)).Returns((User?)null);

            // Act
            _userBusiness.GetById(1);

            // Assert
            _userRepoMock.Verify(x => x.Get(1), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public void Update_PostojeciKorisnik_VracaTrue()
        {
            // Arrange
            var dbUser = new User { IdUser = 1, FirstName = "Staro", LastName = "Prezime", Email = "staro@test.com" };
            var dto = new UserDTO { Id = 1, FirstName = "Novo", LastName = "NovoPrezime", Email = "novo@test.com" };

            _userRepoMock.Setup(x => x.Get(1)).Returns(dbUser);

            // Act
            var result = _userBusiness.Update(dto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Update_NepostojeciKorisnik_VracaFalse()
        {
            // Arrange
            var dto = new UserDTO { Id = 999, FirstName = "X", LastName = "Y", Email = "x@test.com" };

            _userRepoMock.Setup(x => x.Get(999)).Returns((User?)null);

            // Act
            var result = _userBusiness.Update(dto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Update_IzmenjeniPodaciSuProsledjeniRepozitorijumu()
        {
            // Arrange
            var dbUser = new User { IdUser = 1, FirstName = "Staro", LastName = "Prezime", Email = "staro@test.com" };
            var dto = new UserDTO { Id = 1, FirstName = "Novo", LastName = "NovoPrezime", Email = "novo@test.com" };

            _userRepoMock.Setup(x => x.Get(1)).Returns(dbUser);

            // Act
            _userBusiness.Update(dto);

            // Assert
            Assert.Equal("Novo", dbUser.FirstName);
            Assert.Equal("NovoPrezime", dbUser.LastName);
            Assert.Equal("novo@test.com", dbUser.Email);
            _userRepoMock.Verify(x => x.Update(dbUser), Times.Once);
        }

        [Fact]
        public void Update_NepostojeciKorisnik_UpdateNijePozvano()
        {
            // Arrange
            var dto = new UserDTO { Id = 999 };
            _userRepoMock.Setup(x => x.Get(999)).Returns((User?)null);

            // Act
            _userBusiness.Update(dto);

            // Assert
            _userRepoMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_BilokojId_VracaTrue()
        {
            // Arrange
            _userRepoMock.Setup(x => x.Delete(1));

            // Act
            var result = _userBusiness.Delete(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_ProveriDaLiJeRepositoryMetodaPozvana()
        {
            // Arrange
            _userRepoMock.Setup(x => x.Delete(5));

            // Act
            _userBusiness.Delete(5);

            // Assert
            _userRepoMock.Verify(x => x.Delete(5), Times.Once);
        }

        #endregion
    }

    // =====================================================================
    // AuthBusiness Tests
    // =====================================================================
    public class AuthBusinessTests
    {
        private readonly Mock<IRepositoryUser> _userRepoMock;
        private readonly AuthBusiness _authBusiness;

        public AuthBusinessTests()
        {
            _userRepoMock = new Mock<IRepositoryUser>();
            _authBusiness = new AuthBusiness(_userRepoMock.Object);
        }

        #region Authenticate

        [Fact]
        public void Authenticate_IspravniPodaci_VracaUserDTO()
        {
            // Arrange
            var user = new User
            {
                IdUser = 1,
                IdRole = 3,
                FirstName = "Ana",
                LastName = "Anić",
                Email = "ana@test.com",
                PasswordHash = "pass123"
            };

            _userRepoMock.Setup(x => x.GetByEmail("ana@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("ana@test.com", "pass123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Ana", result.FirstName);
            Assert.Equal("Anić", result.LastName);
            Assert.Equal("ana@test.com", result.Email);
        }

        [Fact]
        public void Authenticate_StudentRole_VracaRoleName_Student()
        {
            // Arrange
            var user = new User { IdUser = 1, IdRole = 3, Email = "ana@test.com", PasswordHash = "pass" };
            _userRepoMock.Setup(x => x.GetByEmail("ana@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("ana@test.com", "pass");

            // Assert
            Assert.Equal("Student", result!.RoleName);
        }

        [Fact]
        public void Authenticate_ProfesorRole_VracaRoleName_Profesor()
        {
            // Arrange
            var user = new User { IdUser = 2, IdRole = 2, Email = "marko@test.com", PasswordHash = "pass" };
            _userRepoMock.Setup(x => x.GetByEmail("marko@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("marko@test.com", "pass");

            // Assert
            Assert.Equal("Profesor", result!.RoleName);
        }

        [Fact]
        public void Authenticate_AdminRole_VracaRoleName_Admin()
        {
            // Arrange
            var user = new User { IdUser = 3, IdRole = 1, Email = "admin@test.com", PasswordHash = "admin123" };
            _userRepoMock.Setup(x => x.GetByEmail("admin@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("admin@test.com", "admin123");

            // Assert
            Assert.Equal("Admin", result!.RoleName);
        }

        [Fact]
        public void Authenticate_PogresnaSifra_VracaNull()
        {
            // Arrange
            var user = new User { IdUser = 1, Email = "ana@test.com", PasswordHash = "tacna" };
            _userRepoMock.Setup(x => x.GetByEmail("ana@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("ana@test.com", "pogresna");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_NepostojeciEmail_VracaNull()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByEmail("nema@test.com")).Returns((User?)null);

            // Act
            var result = _authBusiness.Authenticate("nema@test.com", "pass");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_PraznaLozinka_VracaNull()
        {
            // Arrange
            var user = new User { Email = "ana@test.com", PasswordHash = "pass123" };
            _userRepoMock.Setup(x => x.GetByEmail("ana@test.com")).Returns(user);

            // Act
            var result = _authBusiness.Authenticate("ana@test.com", "");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Register

        [Fact]
        public void Register_NovKorisnik_VracaTrue()
        {
            // Arrange
            var user = new User { Email = "novi@test.com", FirstName = "Novi", LastName = "Korisnik" };
            _userRepoMock.Setup(x => x.GetAll()).Returns(new List<User>());

            // Act
            var result = _authBusiness.Register(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Register_NovKorisnik_AddJePozvano()
        {
            // Arrange
            var user = new User { Email = "novi@test.com" };
            _userRepoMock.Setup(x => x.GetAll()).Returns(new List<User>());

            // Act
            _authBusiness.Register(user);

            // Assert
            _userRepoMock.Verify(x => x.Add(user), Times.Once);
        }

        [Fact]
        public void Register_PostojeciEmail_VracaFalse()
        {
            // Arrange
            var user = new User { Email = "postoji@test.com" };
            _userRepoMock.Setup(x => x.GetAll()).Returns(new List<User>
            {
                new User { Email = "postoji@test.com" }
            });

            // Act
            var result = _authBusiness.Register(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Register_PostojeciEmail_AddNijePozvano()
        {
            // Arrange
            var user = new User { Email = "postoji@test.com" };
            _userRepoMock.Setup(x => x.GetAll()).Returns(new List<User>
            {
                new User { Email = "postoji@test.com" }
            });

            // Act
            _authBusiness.Register(user);

            // Assert
            _userRepoMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void Register_ViseKorisnika_SamoNovimEmailom_VracaTrue()
        {
            // Arrange
            var user = new User { Email = "noviji@test.com" };
            _userRepoMock.Setup(x => x.GetAll()).Returns(new List<User>
            {
                new User { Email = "prvi@test.com" },
                new User { Email = "drugi@test.com" }
            });

            // Act
            var result = _authBusiness.Register(user);

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}