using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Security.WS.Entities.Tests
{
    [TestClass]
    public class UserCollectionTests
    {
        [TestMethod]
        public void UserCollection_Add_ShouldAddUserToCollection()
        {
            // Arrange
            var collection = new UserCollection();
            var user = new User { UserID = "1" };

            // Act
            int index = collection.Add(user);

            // Assert
            Assert.AreEqual(0, index);
            Assert.AreEqual(user, collection[0]);
        }

        [TestMethod]
        public void UserCollection_Remove_ShouldRemoveUserFromCollection()
        {
            // Arrange
            var collection = new UserCollection();
            var user = new User { UserID = "1" };
            collection.Add(user);

            // Act
            collection.Remove(user);

            // Assert
            Assert.AreEqual(-1, collection.IndexOf(user));
        }

        [TestMethod]
        public void UserCollection_Contains_ShouldReturnTrueIfUserExists()
        {
            // Arrange
            var collection = new UserCollection();
            var user = new User { UserID = "1" };
            collection.Add(user);

            // Act
            bool contains = collection.Contains(user);

            // Assert
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void UserCollection_IndexOf_ShouldReturnCorrectIndex()
        {
            // Arrange
            var collection = new UserCollection();
            var user1 = new User { UserID = "1" };
            var user2 = new User { UserID = "2" };
            collection.Add(user1);
            collection.Add(user2);

            // Act
            int index = collection.IndexOf(user2);

            // Assert
            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void UserCollection_Insert_ShouldInsertUserAtIndex()
        {
            // Arrange
            var collection = new UserCollection();
            var user1 = new User { UserID = "1" };
            var user2 = new User { UserID = "2" };
            collection.Add(user1);

            // Act
            collection.Insert(1, user2);

            // Assert
            Assert.AreEqual(user2, collection[1]);
        }

        [TestMethod]
        public void UserCollection_Set_ShouldReplaceUserAtIndex()
        {
            // Arrange
            var collection = new UserCollection();
            var user1 = new User { UserID = "1" };
            var user2 = new User { UserID = "2" };
            collection.Add(user1);

            // Act
            collection[0] = user2;

            // Assert
            Assert.AreEqual(user2, collection[0]);
        }
    }
}