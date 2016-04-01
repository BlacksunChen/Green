using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic.Extensions;
using System;

namespace Green
{
    public class CellSpacePartition<Entity> : MonoBehaviour, IEnumerable<Entity>// IEnumerator<Entity>, IEnumerable
        where Entity : Base2DEntity
    {
        /// <summary>
        /// //the required amount of cells in the space
        /// </summary>
        List<Cell<Entity>> _cells;

        /// <summary>
        /// this is used to store any valid neighbors when an agent searches
        /// its neighboring space
        /// </summary>
        List<Entity> _neightbors;

        /// <summary>
        /// this iterator will be used by the methods next and begin to traverse
        /// through the above vector of neighbors
        /// </summary>
        //int _curNeighborIdx;
        //Entity _curNeighbor;
        /// <summary>
        /// the width and height of the world space the entities inhabit
        /// </summary>
        float _spaceWidth;
        float _spaceHeight;

        /// <summary>
        /// the number of cells the space is going to be divided up into
        /// </summary>
        int _numCellsX;
        int _numCellsY;

        float _cellSizeX;
        float _cellSizeY;

        /*
        public Entity Current
        {
            get
            {
                return _curNeighbor;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        */
        /// <summary>
        /// given a position in the game space this method determines the           
        /// relevant cell's index
        /// </summary>

        int PositionToIndex(Vector2 pos)
        {
            int idx = (int)(_numCellsX * pos.x / _spaceWidth) +
            ((int)((_numCellsY) * pos.y / _spaceHeight) * _numCellsX);

            //if the entity's position is equal to vector2d(m_dSpaceWidth, m_dSpaceHeight)
            //then the index will overshoot. We need to check for this and adjust
            if (idx > _cells.Count - 1) idx = _cells.Count - 1;

            return idx;
        }
        public CellSpacePartition(float width,
                                  float height,
                                  int cellsX,
                                  int cellsY,
                                  int MaxEntitys)
        {
            _spaceWidth = width;
            _spaceHeight = height;
            _cellSizeX = cellsX;
            _cellSizeY = cellsY;
            _neightbors = new List<Entity>();

            Vector2 boundSize = new Vector2(cellsX, cellsY);
            //lb->rt
            //create the cells
            for (int y = 0; y < _numCellsY; ++y)
            {
                for (int x = 0; x < _numCellsX; ++x)
                {
                    float right = x * _cellSizeX;
                    float top = y * _cellSizeY;
                    float centerX = right - _cellSizeX / 2;
                    
                    float centerY = top - _cellSizeY / 2;

                    _cells.Add(new Cell<Entity>(new Vector2(centerX, centerY), boundSize));
                }
            }
        }

        /// <summary>
        ///  This must be called to create the vector of neighbors.This method 
        ///  examines each cell within range of the target, If the 
        ///  cells contain entities then they are tested to see if they are situated
        ///  within the target's neighborhood region. If they are they are added to
        ///  neighbor list
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <param name="queryRadius"></param>
        public void CalculateNeighbors(Vector2 TargetPos, float queryRadius)
        {
            //create an iterator and set it to the beginning of the neighbor vector
            //std::vector<entity>::iterator curNbor = m_Neighbors.begin();
            _neightbors.Clear();
           // List<Entity> curNbor = new List<Entity>();
            //create the query box that is the bounding box of the target's query
            //area
            Bounds QueryBox = new Bounds(TargetPos, new Vector2(queryRadius * 2, queryRadius * 2));

            //iterate through each cell and test to see if its bounding box overlaps
            //with the query box. If it does and it also contains entities then
            //make further proximity tests.
            //List<Cell<Entity>> _curCell;
            foreach(var curCell in _cells)
            {
                //test to see if this cell contains members and if it overlaps the
                //query box
                if (curCell.BoundBox.Intersects(QueryBox) &&
                   !(curCell.Members.Count == 0))
                {
                    //add any entities found within query radius to the neighbor list
                    foreach(var it in curCell.Members)
                    
                    {
                        if (it.Position.SqrDistance(TargetPos) <
                            queryRadius * queryRadius)
                        {
                            _neightbors.Add(it);
                        }
                    }
                }
            }//next cell
        }
        
        public void EmptyCells()
        {
            foreach(var item in _cells)
            {
                item.Members.Clear();
            }
        }
        /// <summary>
        /// adds entities to the class by allocating them to the appropriate cell
        /// </summary>
        /// <param name="ent"></param>
        public void AddEntity(Entity ent)
        {
            if(ent != null)
            {
                Debug.LogError("Add Entity null");
            }

            int sz = _cells.Count;
            int idx = PositionToIndex(ent.Position);

            _cells[idx].Members.AddLast(ent);
        }

        /// <summary>
        /// Checks to see if an entity has moved cells. If so the data structure
        /// is updated accordingly
        /// </summary>
        public void UpdateEntity(Entity ent, Vector2 oldPos)
        {
            //if the index for the old pos and the new pos are not equal then
            //the entity has moved to another cell.
            int OldIdx = PositionToIndex(oldPos);
            int NewIdx = PositionToIndex(ent.Position);

            if (NewIdx == OldIdx) return;

            //the entity has moved into another cell so delete from current cell
            _cells[OldIdx].Members.Remove(ent);
            _cells[NewIdx].Members.AddLast(ent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return ((IEnumerable<Entity>)_neightbors).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Entity>)_neightbors).GetEnumerator();
        }

        /*
public bool MoveNext()
{
   if(++_curNeighborIdx >= _neightbors.Count)
   {
       return false;
   }
   else
   {
       _curNeighbor = _neightbors[_curNeighborIdx];
   }
   return true;
}

public void Reset()
{
   _curNeighborIdx = -1;
}

public void Dispose()
{

}
*/
    }
}