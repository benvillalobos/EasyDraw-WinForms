using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDrawLib
{
    /// <summary>
    /// The collection of all sprites being drawn
    /// </summary>
    public class GraphicsItemsCollection : IList<GraphicsItem>
    {
        List<GraphicsItem> _drawItems = new List<GraphicsItem>();
        Dictionary<string, int> _drawItemNames = new Dictionary<string, int>();
        /// <summary>
        /// Gets the index of a sprite
        /// </summary>
        /// <param name="item">The item to get the index of</param>
        /// <returns></returns>
        public int IndexOf(GraphicsItem item)
        {
            return _drawItems.IndexOf(item);
        }
        /// <summary>
        /// Inserts a sprite into the collection
        /// </summary>
        /// <param name="index">The index to insert into</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, GraphicsItem item)
        {
            _drawItems.Insert(index, item);

            if (item != null && String.IsNullOrEmpty(item.Name))
            {
                //Check if the name is available
                if (_drawItemNames.ContainsKey(item.Name))
                {
                    throw new Exception(String.Format("There is already an object named {0}; please choose a different name", item.Name));
                }

                _drawItemNames.Add(item.Name, index);
            }
        }
        /// <summary>
        /// Removes a specific sprite at a particular spot
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _drawItemNames.Remove(_drawItems[index].Name);
            _drawItems.RemoveAt(index);
        }
        /// <summary>
        /// Gets a sprite at a particular index
        /// </summary>
        /// <param name="index">The index to get the item from</param>
        /// <returns></returns>
        public GraphicsItem this[int index]
        {
            get
            {
                return _drawItems[index];
            }
            set
            {
                _drawItems[index] = value;
            }
        }
        /// <summary>
        /// Gets a sprite with a particular name
        /// </summary>
        /// <param name="name">The name to look for</param>
        /// <returns></returns>
        public GraphicsItem this[string name]
        {
            get
            {
                if (!_drawItemNames.ContainsKey(name))
                {
                    throw new KeyNotFoundException(string.Format("There is no sprite named {0}", name));
                }
                return _drawItems[_drawItemNames[name]];
            }
        }
        /// <summary>
        /// Adds a sprite to the collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(GraphicsItem item)
        {
            _drawItems.Add(item);
            if (!string.IsNullOrEmpty(item.Name))
            {
                if (_drawItemNames.ContainsKey(item.Name))
                {
                    throw new Exception(string.Format("There is already an item named {0}", item.Name));
                }
                _drawItemNames.Add(item.Name, _drawItems.Count - 1);
            }
        }
        /// <summary>
        /// Adds a sprite with a name
        /// </summary>
        /// <param name="item">The sprite to add</param>
        /// <param name="name">The name to associate it with</param>
        public void Add(GraphicsItem item, string name)
        {
            _drawItems.Add(item);
            _drawItemNames.Add(name, _drawItems.Count-1);
        }
        /// <summary>
        /// Clears the entire list of sprites and names
        /// </summary>
        public void Clear()
        {
            _drawItems.Clear();
            _drawItemNames.Clear();
        }
        /// <summary>
        /// Checks if a sprite is in the collection
        /// </summary>
        /// <param name="item">The sprite to add</param>
        /// <returns></returns>
        public bool Contains(GraphicsItem item)
        {
            return _drawItems.Contains(item);
        }
        /// <summary>
        /// Checks if a sprite with a particular name is in the collection
        /// </summary>
        /// <param name="name">The name of the sprite to look for</param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return _drawItemNames.ContainsKey(name);
        }

        /// <summary>
        /// Copies the entire List to a compatible one-dimensionalarray, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The array to copy into</param>
        /// <param name="arrayIndex">The index to start with</param>
        public void CopyTo(GraphicsItem[] array, int arrayIndex)
        {
            _drawItems.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// The number of items in the collection
        /// </summary>
        public int Count
        {
            get { return _drawItems.Count; }
        }
        
        /// <summary>
        /// Not implemented
        /// </summary>
        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Removes a particular sprite from the collection
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns></returns>
        public bool Remove(GraphicsItem item)
        {
            _drawItemNames.Remove(item.Name);
            return _drawItems.Remove(item);
        }
        /// <summary>
        /// Removes a sprite with a particular name from the collection
        /// </summary>
        /// <param name="name">The name of the sprite to remove</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            _drawItems.Remove(_drawItems[_drawItemNames[name]]);
            return _drawItemNames.Remove(name);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<GraphicsItem> GetEnumerator()
        {
            return _drawItems.AsEnumerable<GraphicsItem>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _drawItems.GetEnumerator();
        }
    }
}
