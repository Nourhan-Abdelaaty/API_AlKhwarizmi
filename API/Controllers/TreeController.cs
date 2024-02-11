using API.Helper;
using API.Models;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace API.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {

       private readonly ITreeService _TreeService;
       public TreeController(ITreeService TreeService) =>
           _TreeService = TreeService;


       [HttpGet("{id}")]
       public async Task<ResponseResult> GetNode(string id)
       {
           return await _TreeService.GetNode(id);
       }
     
       [HttpPost]
       public async Task<ResponseResult> AddNode(TreeItem node)
       {
          return await _TreeService.AddNode(node);
       }
       [HttpPut]
       public async Task<ResponseResult> UpdateNode( TreeItem updatedNode)
       {
           return await _TreeService.UpdateNode(updatedNode);
       }
      
       [HttpDelete("{id}")]
       public async Task<ResponseResult> DeleteNode(string id)
       {
           return await _TreeService.DeleteNode(id);
       }
       [HttpGet("build")]
       public async Task<ResponseResult> BuildTree()
       {
           return await _TreeService.BuildTree();
       } 
       [HttpGet("GetAll")]
       public async Task<ResponseResult> GetAll()
       {
           return await _TreeService.GetAll();
       }
}      
       