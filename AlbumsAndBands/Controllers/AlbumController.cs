using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZavrsniTest.Repository.Interfaces;
using ZavrsniTest.Models;
using ZavrsniTest.Models.DTO;
using System.Linq;

namespace ZavrsniTest.Controllers
{
    [Route("api/albumi")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumController(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        // GET: api/Albums
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAlbums()
        {
            return Ok(_albumRepository.GetAll().ProjectTo<AlbumDTO>(_mapper.ConfigurationProvider).OrderBy(a => a.Name).ToList());
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAlbum(int id)
        {
            var album = _albumRepository.GetById(id);
            if (album == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AlbumDTO>(album));
        }


        // PUT: api/Albums/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.Id)
            {
                return BadRequest();
            }

            try
            {
                _albumRepository.Update(album);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(album);
        }

        // POST: api/Albums
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _albumRepository.Add(album);
            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteAlbum(int id)
        {
            var album = _albumRepository.GetById(id);
            if (album == null)
            {
                return NotFound();
            }

            _albumRepository.Delete(album);
            return NoContent();
        }

        [HttpPost]
        [Route("pretraga/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Search(SearchDTO dto)
        {
            if(dto.Maximum < 0 || dto.Minimum < 0 || dto.Minimum > dto.Maximum)
            {
                return BadRequest();
            }
            return Ok(_albumRepository.GetAllByParameters(dto.Minimum, dto.Maximum).ProjectTo<AlbumDTO>(_mapper.ConfigurationProvider).ToList());
        }
    }
}
