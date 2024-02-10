using API.Helper;
using API.Models;
using Microsoft.AspNetCore.Mvc;
namespace API.Services.Interfaces; 
public interface ITreeService
{
    Task<ResponseResult> AddNode(TreeItem node);
    Task<ResponseResult> UpdateNode( TreeItem updatedNode);
    Task<ResponseResult> DeleteNode(string id);
    Task<ResponseResult> GetNode(string id);
    Task<ResponseResult> GetAll();
    Task<ResponseResult> BuildTree();
}