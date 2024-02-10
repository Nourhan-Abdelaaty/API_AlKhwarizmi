using API.Helper;
using API.Models;
using API.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Xml.Linq;

namespace API.Services.Implementation;
public class TreeService : ITreeService
{
    private readonly IMongoCollection<TreeItem> _treeCollection;
    public TreeService(
        IOptions<TreeDatabaseSetting> TreeDatabaseSetting)
    {
        var mongoClient = new MongoClient(
            TreeDatabaseSetting.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            TreeDatabaseSetting.Value.DatabaseName);

        _treeCollection = mongoDatabase.GetCollection<TreeItem>(
            TreeDatabaseSetting.Value.TreeCollectionName);
    }

    public async Task<ResponseResult> AddNode(TreeItem node)
    {
        try
        {
            await _treeCollection.InsertOneAsync(node);
            return new ResponseResult
            { isSuccssed = true, message = "تم الحفظ بنجاح", obj = node };
        }
        catch(Exception ex)
        {
            return new ResponseResult
            { isSuccssed = false, message = ex.Message };
        }
    }
    public async Task<ResponseResult> UpdateNode(TreeItem updatedNode)
    {
        try
        {
            var existingNode = await _treeCollection.Find(n => n.id == updatedNode.id).FirstOrDefaultAsync();
            if (existingNode == null)
            {
                return new ResponseResult { isSuccssed = false, message = "لم يتم التعرف" };
            }
            await _treeCollection.ReplaceOneAsync(n => n.id == updatedNode.id, updatedNode);
            return new ResponseResult { isSuccssed = true, message = "تم التعديل بنجاح", obj = updatedNode };
        }
        catch (Exception ex)
        {
            return new ResponseResult
            { isSuccssed = false, message = ex.Message };
        }
    }
    public async Task<ResponseResult> DeleteNode(string id)
    {
        try
        {
            var deleteResult = await _treeCollection.DeleteOneAsync(n => n.id == id);
            if (deleteResult.DeletedCount == 0)
            {
                return new ResponseResult { isSuccssed = false, message = "لم يتم التعرف" };
            }
            return new ResponseResult { isSuccssed = true, message = "تم الحذف بنجاح", obj = deleteResult };
        }
        catch (Exception ex)
        {
            return new ResponseResult
            { isSuccssed = false, message = ex.Message };
        }
    }
    public async Task<ResponseResult> GetNode(string id)
    {
     try
     { 
        var node = await _treeCollection.Find(n => n.id == id).FirstOrDefaultAsync();
        if (node == null)
        {
            return new ResponseResult { isSuccssed = false, message = "لم يتم التعرف" };
        }
        return new ResponseResult { isSuccssed = true, message = "تم الاسترجاع بنجاح", obj = node };
      }
     catch(Exception ex)
     {
         return new ResponseResult { isSuccssed = false, message = ex.Message };
     }
    }
    public async Task<ResponseResult> GetAll()
    {
        var AllData = await _treeCollection.Find(n => n.parentId == null).ToListAsync();
        return new ResponseResult { isSuccssed = true, message = "تم الاسترجاع بنجاح", obj = AllData };
    }
    public async Task<ResponseResult> BuildTree()
    {
        try
        {
            var rootNodes = await _treeCollection.Find(n => n.parentId == null).ToListAsync();
            foreach (var rootNode in rootNodes)
            {
                BuildSubtree(rootNode);
            }
            return new ResponseResult { isSuccssed = true, message = "تم الاسترجاع بنجاح", obj = rootNodes };
        }
        catch (Exception ex)
        {
            return new ResponseResult
            { isSuccssed = false, message = ex.Message };
        }
    }
    private void BuildSubtree(TreeItem node)
    {
        var children = _treeCollection.Find(n => n.parentId == node.id).ToList();
        node.children = children;
        foreach (var child in children)
        {
            BuildSubtree(child);
        }
    }
}

