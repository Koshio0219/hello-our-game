﻿using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Data;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.Unit
{
    public class NormalBlock : BlockBase
    {
        public Transform createPoint;

        public List<Transform> curTriggerEnter = new();
        private static bool isDraging = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isDraging) return;
            if (other.transform.parent == createPoint || 
                other.transform.parent == transform ||
                other.transform.parent == transform.parent) 
                return;

            var up = other.transform.GetRootParent();
            if (curTriggerEnter.Contains(up)) return;
            curTriggerEnter.Add(up);
        }

        private void OnTriggerExit(Collider other)
        {
            if (isDraging) return;

            if (curTriggerEnter.Contains(other.transform))
            {
                curTriggerEnter.Remove(other.transform);
                return;
            }

            var up = other.transform.GetRootParent();
            if (curTriggerEnter.Contains(up))
            {
                curTriggerEnter.Remove(up);
            }
        }

        public override void OnMovingStart()
        {
            base.OnMovingStart();
            isDraging = true;
            if (curTriggerEnter.Count == 0) return;
            curTriggerEnter.ForEach(one => 
            {
                var colliders = one.GetComponentsInChildren<Collider>().ToList();
                colliders.ForEach(c => c.enabled = false);
                if (one.TryGetComponent<NavMeshAgent>(out var agent)) agent.isStopped = true;
                one.SetParent(createPoint);
            });
        }

        public override async void OnMovingEnd()
        {
            base.OnMovingEnd();
            await UniTask.Delay(500);
            curTriggerEnter.ForEach(one => 
            {
                var lastPos = one.transform.position;
                one.SetParent(null);
                one.transform.position = lastPos;
                var colliders = one.GetComponentsInChildren<Collider>().ToList();
                colliders.ForEach(c => c.enabled = true);
                if (one.TryGetComponent<NavMeshAgent>(out var agent)) agent.isStopped = false;
            });
            await UniTask.Delay(100);
            isDraging = false;
        }
    }
}

