using NUnit.Framework;
using OpenB.Core.ACL;
using Rhino.Mocks;

namespace OpenB.Modeling.Test
{
    [TestFixture]
    public class ModelAccessTest
    {
        [Test]
        public void Model_UserIsNotInGroup_GroupHasNoPermissions_UserHasNoPermissions_PermissionsAreNotOK()
        {
            UserGroup firstUserGroup = new UserGroup("MyFirstUserGroup", "My first usergroup", "My first usergroup description");
            UserGroup secondUserGroup = new UserGroup("MySecondUserGroup", "My second usergroup", "My second usergroup description");
            User owner = new User("MyUser", firstUserGroup);

            MockRepository mockRepository = new MockRepository();
            IAuthorizebleModel model = mockRepository.Stub<IAuthorizebleModel>();

            model.Stub(m => m.User).Return(owner);
            model.Stub(m => m.GroupPermissions).Return(Permissions.Read | Permissions.Write);
            model.Stub(m => m.UserPermissions).Return(Permissions.Read | Permissions.Write);
            model.Stub(m => m.UserGroup).Return(secondUserGroup);

            mockRepository.ReplayAll();

            ModelAuthorizationService authorizationService = new ModelAuthorizationService();

            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Read, model), Is.False);
            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Write, model), Is.False);
        }

        [Test]
        public void Model_UserIsInGroup_GroupHasPermissions_UserHasPermissions_PermissionsAreOK()
        {
            UserGroup userGroup = new UserGroup("MyFirstUserGroup", "My first usergroup", "My first usergroup description");
            User owner = new User("MyUser", userGroup);

            MockRepository mockRepository = new MockRepository();
            IAuthorizebleModel model = mockRepository.Stub<IAuthorizebleModel>();

            model.Stub(m => m.User).Return(owner);
            model.Stub(m => m.GroupPermissions).Return(Permissions.Read | Permissions.Write);
            model.Stub(m => m.UserPermissions).Return(Permissions.Read | Permissions.Write);
            model.Stub(m => m.UserGroup).Return(userGroup);

            mockRepository.ReplayAll();

            ModelAuthorizationService authorizationService = new ModelAuthorizationService();
            
            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Read, model), Is.True);
            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Write, model), Is.True);
        }

        [Test]
        public void Model_UserIsInGroup_GroupHasNoPermissions_UserHasPermissions_PermissionsAreNotOK()
        {
            UserGroup userGroup = new UserGroup("MyFirstUserGroup", "My first usergroup", "My first usergroup description");
            User owner = new User("MyUser", userGroup);

            MockRepository mockRepository = new MockRepository();
            IAuthorizebleModel model = mockRepository.Stub<IAuthorizebleModel>();

            model.Stub(m => m.User).Return(owner);
            model.Stub(m => m.GroupPermissions).Return(Permissions.Read);
            model.Stub(m => m.UserPermissions).Return(Permissions.Read);
            model.Stub(m => m.UserGroup).Return(userGroup);

            mockRepository.ReplayAll();

            ModelAuthorizationService authorizationService = new ModelAuthorizationService();

            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Read, model), Is.True);
            Assert.That(authorizationService.IsUserAuthorizedForModel(owner, Permissions.Write, model), Is.False);


        }
    }
}