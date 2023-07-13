using NUnit.Framework;
using webapi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Utils.Tests
{
    [TestFixture()]
    public class CommentValidationAttributeTests
    {
        [Test()]
        public void NullCommentInListTest()
        {
            // Arrange
            var attribute = new CommentValidationAttribute();
            var commentlList = new List<Comment> {
                new Comment
                {
                    Author = "John Doe",
                    Text = "This is a random comment.",
                    CreatedAt = DateTime.Now
                },
                null
            };

            // Act
            var isValid = attribute.IsValid(commentlList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void EmptyListTest()
        {
            // Arrange
            var attribute = new CommentValidationAttribute();
            var commentlList = new List<Comment> {};

            // Act
            var isValid = attribute.IsValid(commentlList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void ValidCommentListTest()
        {
            // Arrange
            var attribute = new CommentValidationAttribute();
            var commentlList = new List<Comment> {
                new Comment
                {
                    Author = "John Doe",
                    Text = "This is a random comment.",
                    CreatedAt = DateTime.Now
                },
                new Comment
                {
                    Author = "Martin",
                    Text = "This is a random comment.",
                    CreatedAt = DateTime.Now
                }
            };

            // Act
            var isValid = attribute.IsValid(commentlList);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test()]
        public void NullListTest()
        {
            // Arrange
            var attribute = new CommentValidationAttribute();
            List<Comment> commentlList = null;

            // Act
            var isValid = attribute.IsValid(commentlList);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}