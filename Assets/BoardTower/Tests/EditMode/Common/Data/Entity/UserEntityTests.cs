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
        public void SetDisplayName_PreservesProgresses()
        {
            var progresses = new[] { new ProgressVO(1, 5) };
            _entity.Set(CreateUserVOWithProgresses(progresses));
            var newName = new UserDisplayNameVO("NewName");

            _entity.SetDisplayName(newName);

            Assert.That(_entity.value.playFabUser.progresses, Is.EqualTo(progresses));
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

        [Test]
        public void Find_WhenProgressExists_ReturnsMatchingProgress()
        {
            var entity = new UserEntity();
            var progress = new ProgressVO(1, 5);
            entity.Set(CreateUserVOWithProgresses(new[] { progress }));

            var result = entity.Find(1);

            Assert.That(result.type, Is.EqualTo(1));
            Assert.That(result.value, Is.EqualTo(5));
        }

        [Test]
        public void Find_WhenProgressNotExists_ReturnsDefaultProgressWithZeroValue()
        {
            var entity = new UserEntity();
            entity.Set(CreateUserVOWithProgresses(new ProgressVO[0]));

            var result = entity.Find(2);

            Assert.That(result.type, Is.EqualTo(2));
            Assert.That(result.value, Is.EqualTo(0));
        }

        private static UserVO CreateUserVO(string displayName)
        {
            var localUser = new LocalUserVO("test-id");
            UserDisplayNameVO userDisplayName = displayName != null
                ? new UserDisplayNameVO(displayName)
                : UserDisplayNameVO.Create();
            var playFabUser = new PlayFabUserVO(false, userDisplayName, new ProgressVO[0]);
            return new UserVO(localUser, playFabUser);
        }

        private static UserVO CreateUserVOWithProgresses(ProgressVO[] progresses)
        {
            var localUser = new LocalUserVO("test-id");
            var playFabUser = new PlayFabUserVO(false, UserDisplayNameVO.Create(), progresses);
            return new UserVO(localUser, playFabUser);
        }
    }
}
