using Common.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Tests.Extensions
{
    public class ImageExtensionTests
    {
        [Test]
        public void ImageExtensionTestsSimplePasses()
        {
            // Arrange
            var gameObject = new GameObject("Image");
            var imageComp = gameObject.AddComponent<Image>();
            var color = Color.black;
            imageComp.color = color;
            var alpha = .25f;

            // Act
            imageComp.SetColorAlpha(alpha);

            // Assert
            Assert.AreEqual(color.r, imageComp.color.r);
            Assert.AreEqual(color.g, imageComp.color.g);
            Assert.AreEqual(color.b, imageComp.color.b);
            Assert.AreEqual(alpha, imageComp.color.a);
        }
    }
}