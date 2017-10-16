using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using System;
using RPG.Characters; //So we can detect by type

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Texture2D NPCCursor;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 8;

        float maxRaycastDepth = 100f; // Hard coded value


        //new delegates
        //OnMouseOverEnemy

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverNPC(TalkToNPC npc);
        public event OnMouseOverNPC onMouseOverNPC;

        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Implemen UI interaction
            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            //if(ScreenAtConstruction.Contains(Input.mousePosition))
            //{
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Specify layer priorities here

                if (RaycastForEnemy(ray)) { return; }
                if (RacastForNPC(ray)) { return; }
                if (RaycastForPotentiallyWalkable(ray)) { return; }
            //}
            
        }

        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameobjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameobjectHit.GetComponent<EnemyAI>();
            if(enemyHit)
            {
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        private bool RaycastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if(potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
            
        }

        private bool RacastForNPC(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameobjectHit = hitInfo.collider.gameObject;
            var NPCHit = gameobjectHit.GetComponent<TalkToNPC>();
            if (NPCHit)
            {
                print(gameobjectHit.name);
                Cursor.SetCursor(NPCCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverNPC(NPCHit);
                return true;
            }
            return false;

        }
    }
}
