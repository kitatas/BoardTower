using BoardTower.Common.Application;
using BoardTower.Common.Data.Entity;
using NUnit.Framework;

namespace BoardTower.Tests.EditMode.Common.Data.Entity
{
    [TestFixture]
    public sealed class UserEntityTests
    {
        private UserEntity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new UserEntity();
            _entity.Set(CreateUserVO("TestUser"));
        }

        [Test]
        public void DisplayName_ReturnsPlayFabUserDisplayNameValue()
        {
            Assert.That(_entity.displayName, Is.EqualTo("TestUser"));
        }

        [Test]
        public void IsRegistered_WhenDisplayNameIsNonEmpty_ReturnsTrue()
        {
            Assert.That(_entity.isRegistered, Is.True);
        }

        [Test]
        public void IsRegistered_WhenDisplayNameIsEmpty_ReturnsFalse()
        {
            _entity.Set(CreateUserVO(null));

            Assert.That(_entity.isRegistered, Is.False);
        }

        [Test]
        public void SetDisplayName_UpdatesDisplayName()
        {
            var newName = new UserDisplayNameVO("NewName");

            _entity.SetDisplayName(newName);

            Assert.That(_entity.displayName, Is.EqualTo("NewName"));
        }

        [Test]
        public void SetDisplayName_PreservesLocalUser()
        {
            var newName = new UserDisplayNameVO("NewName");
            var originalLocalUserId = _entity.value.localUser.id;

            _entity.SetDisplayName(newName);

            Assert.That(_entity.value.localUser.id, Is.EqualTo(originalLocalUserId));
        }

        [Test]
        public void SetDisplayName_PreservesIsNewly()
        {
            var newName = new UserDisplayNameVO("NewName");
            var originalIsNewly = _entity.value.playFabUser.isNewly;

            _entity.SetDisplayName(newName);

            Assert.That(_entity.value.playFabUser.isNewly, Is.EqualTo(originalIsNewly));
        }

        [Test]
        public void Set_UpdatesValue()
        {
            var newUser = CreateUserVO("Another");

            _entity.Set(newUser);

            Assert.That(_entity.value, Is.EqualTo(newUser));
        }

        [Test]
        public void IsEqual_WithSameUserVO_ReturnsTrue()
        {
            var user = CreateUserVO("SameName");
            _entity.Set(user);

            Assert.That(_entity.IsEqual(user), Is.True);
        }

        [Test]
        public void IsEqual_WithDifferentUserVO_ReturnsFalse()
        {
            _entity.Set(CreateUserVO("Name1"));

            Assert.That(_entity.IsEqual(CreateUserVO("Name2")), Is.False);
        }

        private static UserVO CreateUserVO(string displayName)
        {
            var localUser = new LocalUserVO("test-id");
            UserDisplayNameVO userDisplayName = displayName != null
                ? new UserDisplayNameVO(displayName)
                : UserDisplayNameVO.Create();
            var playFabUser = new PlayFabUserVO(false, userDisplayName);
            return new UserVO(localUser, playFabUser);
        }
    }
}
