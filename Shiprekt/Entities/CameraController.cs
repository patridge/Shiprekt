﻿using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;

namespace Shiprekt.Entities
{
    public partial class CameraController
    {
        #region Enums

        public enum FollowTargetType
        {
            Entity,
            Position
        }

        #endregion

        float shakeMagnitude = 0;
        float shakeAngle = 0;

        float minX;
        float minY;
        float maxX;
        float maxY;

        public FollowTargetType CurrentFollowTargetType
        {
            get; set;
        }

        public Vector2 TargetPosition { get; set; }

        public PositionedObject TargetEntity { get; set; }

        public bool FollowImmediately { get; set; }

        public Camera Camera { get; set; }

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {


        }

        private void CustomActivity()
        {
            if(FollowImmediately)
            {
                switch(CurrentFollowTargetType)
                {
                    case FollowTargetType.Entity:
                        X = TargetEntity.X;
                        Y = TargetEntity.Y;
                        break;
                    case FollowTargetType.Position:
                        X = TargetPosition.X;
                        Y = TargetPosition.Y;
                        break;
                }
            }

            X = Math.Max(minX, X);
            Y = Math.Max(minY, Y);

            X = Math.Min(maxX, X);
            Y = Math.Min(maxY, Y);

            if (shakeMagnitude > 0)
            {
                Vector2 offset = new Vector2(0, 0);
                offset = new Vector2((float)(Math.Sin(shakeAngle) * shakeMagnitude), (float)(Math.Cos(shakeAngle) * shakeMagnitude));
                shakeMagnitude -= TimeManager.SecondDifference * ShakeMagnitude / ShakeDuration;

                shakeAngle = FlatRedBallServices.Random.AngleRadians();

                Camera.X = X + offset.X;
                Camera.Y = Y + offset.Y;
            }
            else
            {
                Camera.X = X;
                Camera.Y = Y;
            }


        }

        public void SetBordersAtZ(float minY, float maxX)
        {
            this.Camera.UsePixelCoordinates3D(0);

            // this is pixel perfect when resolution is 1080 / 2 = 540
            // If the resolution is smaller, we need to zoom out
            var zMultiple = 540 / (float)Camera.DestinationRectangle.Height;
            this.Camera.Z *= zMultiple;

            Camera.SetBordersAtZ(0, minY, maxX, 0, 0);

            this.minX = Camera.MinimumX + shakeMagnitude;
            this.minY = Camera.MinimumY + shakeMagnitude;
            this.maxX = Camera.MaximumX - shakeMagnitude;
            this.maxY = Camera.MaximumY - shakeMagnitude;

            Camera.ClearBorders();

        }

        public void DoShake()
        {
            shakeMagnitude = ShakeMagnitude;
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
