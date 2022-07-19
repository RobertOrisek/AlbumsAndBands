using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZavrsniTest.Controllers;
using ZavrsniTest.Models;
using ZavrsniTest.Models.DTO;
using ZavrsniTest.Repository.Interfaces;

namespace TestProject.Controllers
{
    public class AlbumsControllerTest
    {
        [Fact]
        public void GetAlbum_VaidId_ReturnsObject()
        {
            Album album = new Album()
            {
                Id = 1,
                BandId = 1,
                Band = new Band() { Id = 1, FoundationYear = 1111, Name = "Band 1" },
                Name = "Album 1",
                CopiesSold = 1,
                Genre = "Genre 1",
                PublishingYear = 1111
            };

            AlbumDTO albumDTO = new AlbumDTO()
            {
                Id = 1,
                PublishingYear = 1111,
                Genre = "Genre 1",
                CopiesSold = 1,
                Name = "Album 1",
                BandName = "Band 1"
            };

            var mockRepository = new Mock<IAlbumRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(album);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new AlbumProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new AlbumController(mockRepository.Object, mapper);

            //Act
            var actionResult = controller.GetAlbum(1) as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            AlbumDTO dtoResult = (AlbumDTO)actionResult.Value;
            Assert.Equal(album.Id, dtoResult.Id);
            Assert.Equal(album.Name, dtoResult.Name);
            Assert.Equal(album.PublishingYear, dtoResult.PublishingYear);
            Assert.Equal(album.Band.Name, dtoResult.BandName);
            Assert.Equal(album.CopiesSold, dtoResult.CopiesSold);
        }

        [Fact]
        public void PutAlbum_InvalidId_ReturnsBadRequest()
        {
            //arrange
            Album album = new Album()
            {
                Id = 1,
                Band = new Band() { Id = 1, FoundationYear = 1111, Name = "Band 1" },
                Name = "Album 1",
                BandId = 1,
                CopiesSold = 1,
                Genre = "Genre 1",
                PublishingYear = 1111
            };

            var mockRepository = new Mock<IAlbumRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(album);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new AlbumProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new AlbumController(mockRepository.Object, mapper);

            //Act
            var actionResult = controller.PutAlbum(12, album) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void DeleteAlbum_InvalidId_ReturnsNotFound()
        {

            //arrange
            Album album = new Album()
            {
                Id = 1,
                Band = new Band() { Id = 1, FoundationYear = 1111, Name = "Band 1" },
                Name = "Album 1",
                BandId = 1,
                CopiesSold = 1,
                Genre = "Genre 1",
                PublishingYear = 1111
            };

            var mockRepository = new Mock<IAlbumRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(album);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new AlbumProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new AlbumController(mockRepository.Object, mapper);

            //Act
            var actionResult = controller.DeleteAlbum(13) as NotFoundResult;

            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void PostAlbum_ValidRequest_SetsLocationHeader()
        {

            //arrange
            Album album = new Album()
            {
                Id = 1,
                Band = new Band() { Id = 1, FoundationYear = 1111, Name = "Band 1" },
                Name = "Album 1",
                BandId = 1,
                CopiesSold = 1,
                Genre = "Genre 1",
                PublishingYear = 1111
            };

            var mockRepository = new Mock<IAlbumRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(album);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new AlbumProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new AlbumController(mockRepository.Object, mapper);

            //Act
            var actionResult = controller.PostAlbum(album) as CreatedAtActionResult;


            //Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);
            Assert.Equal("GetAlbum", actionResult.ActionName);
            Assert.Equal(1, actionResult.RouteValues["id"]);

            Album albumResult = (Album)actionResult.Value;
            Assert.Equal(album, albumResult);
        }
    }
}
