using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace TheBackyard.MonoGameLib;

// i need a place to store and manage textures for general objects and things that cannot store their own.
// ideally, this should be as simple as having this function as a central dictionary.
// there should be a queue removal system, so things can create textures and schedule their removal at the end of the draw cycle.

/// <summary>
/// Manage and keep track of game <see cref="Texture2D"/>s.
/// Made to streamline the process of unloading unused textures and clearing up memory wherever possible.
/// </summary>
public class TextureManager
{
    #region public parts
    /// <summary>
    /// A 1x1 <see cref="Color.White"/> <see cref="Texture2D"/>.
    /// </summary>
    public Texture2D PixelTexture { get; private set; }
    /// <summary>
    /// The central dictionary containing all textures and ids
    /// </summary>
    public Dictionary<string, Texture2D> TextureDictionary { get; private set; }
    #endregion public parts
    #region private parts
    private List<string> _removalQueue;
    #endregion private parts

    /// <summary>
    /// Create a new <see cref="TextureManager"/>.
    /// </summary>
    /// <param name="graphicsDevice"></param>
    public TextureManager(GraphicsDevice graphicsDevice)
    {
        PixelTexture = CreatePixelTexture(graphicsDevice);
        TextureDictionary = [];
        _removalQueue = [];
    }

    #region adding items
    /// <summary>
    /// Add a texture with a set <paramref name="id"/> 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="texture"></param>
    public void AddItem(string id, Texture2D texture)
    {
        ArgumentNullException.ThrowIfNull(texture);
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (TextureDictionary.ContainsKey(id))
            throw new ArgumentException($"A texture with ID '{id}' already exists.");

        TextureDictionary[id] = texture;
    }

    /// <summary>
    /// Add a texture without a set ID, and instead have one automatically assigned
    /// </summary>
    /// <returns>the string id to the item</returns>
    public string AddItem(Texture2D texture)
    {
        ArgumentNullException.ThrowIfNull(texture);

        string ID;
        do
        {
            // use the built-in Guid class to create a new identifier
            // i love built-ins sometimes
            ID = Guid.NewGuid().ToString("N");
        } while (TextureDictionary.ContainsKey(ID));

        AddItem(ID, texture);
        return ID;
    }

    /// <summary>
    /// Add an item to the <see cref="TextureManager"/>, only to immediately queue it for deletion.
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public string AddTemporaryItem(Texture2D texture)
    {
        string ID = AddItem(texture);
        QueueRemoval(ID);
        return ID;
    }
    #endregion adding items
    #region managing items
    /// <summary>
    /// Get a texture from the TextureManager
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"><paramref name="ID"/>was not present in the <see cref="TextureDictionary"/>.</exception>
    public Texture2D GetTexture(string ID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ID);
        if (!TextureDictionary.TryGetValue(ID, out Texture2D? texture))
            throw new KeyNotFoundException($"No texture with ID '{ID}' was found.");
        if (texture is null)
        {
            throw new NullReferenceException();
        }
        return texture;
    }

    /// <summary>
    /// Set the data of an existing texture in the dictionary
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="data"></param>
    /// <returns>true if successful, false otherwise</returns>
    /// <remarks>Will fail if the texture does not exist, or if the data is incompatible with the texture</remarks>
    public bool SetData(string ID, Color[] data)
    {
        bool success = false;

        ArgumentNullException.ThrowIfNull(ID);
        ArgumentNullException.ThrowIfNull(data);

        if (TextureDictionary.TryGetValue(ID, out _))
        {
            try
            {
                TextureDictionary[ID].SetData(data);
                success = true;
            }
            catch
            {
                Debug.WriteLine($"[Error] Failed to set data for texture with ID '{ID}'!");
            }
        }

        return success;
    }

    /// <summary>
    /// Dispose all textures in the <see cref="TextureManager"/>
    /// </summary>
    public void DisposeAll()
    {
        foreach (KeyValuePair<string, Texture2D> item in TextureDictionary)
        {
            try
            {
                item.Value.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // only catch ObjectDisposedExceptions. if there's a real error, it should throw.
            }

            TextureDictionary.Clear();
            _removalQueue.Clear();
        }
    }
    #region queue management
    /// <summary>
    /// Queue the removal of a stored Texture2D
    /// </summary>
    /// <param name="id"></param>
    public void QueueRemoval(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        if (TextureDictionary.ContainsKey(id) && !_removalQueue.Contains(id))
            _removalQueue.Add(id);
    }

    /// <summary>
    /// Empty the removal queue and remove all queued items
    /// </summary>
    public void RemoveQueued()
    {
        foreach (string ID in _removalQueue)
        {
            if (TextureDictionary.TryGetValue(ID, out Texture2D? texture))
            {
                try
                {
                    texture.Dispose();
                }
                catch
                {
                    // ignore disposal errors for now, this may change in the future
                }
                TextureDictionary.Remove(ID);
            }
        }
        _removalQueue.Clear();
    }
    #endregion queue management
    #endregion managing items

    /// <summary>
    /// Create a 1x1 white pixel texture.
    /// </summary>
    /// <returns></returns>
    private static Texture2D CreatePixelTexture(GraphicsDevice graphicsDevice)
    {
        Texture2D pixelTexture = new(graphicsDevice, 1, 1);
        pixelTexture.SetData([Color.White]);
        return pixelTexture;
    }
}