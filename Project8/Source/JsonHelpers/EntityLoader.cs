using Project8.Source.Entities.Behaviors;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using static Project8.Source.Entities.Entity;

namespace Project8.Source.JsonHelpers
{
    public static class EntityLoader
    {
        public static int sz = GlobalVariables.tilesize;

        public static Dictionary<string, Entity> Load(string filePath)
        {
            string json = File.ReadAllText(filePath);

            var root = JsonSerializer.Deserialize<RootEntities>(json);

            var result = new Dictionary<string, Entity>();

            foreach (var e in root.entities)
            {
                var entity = new Entity();

                // Behaviors
                entity.Behaviors = e.behaviors.ToArray();

                // Alignment (enum)
                if (Enum.TryParse<Alignments>(e.alignment, true, out var align))
                    entity.Alignment = align;

                // AnimationSpeed
                if (float.TryParse(e.AnimationSpeed?.Replace("F", ""), out float speed))
                    entity.AnimationSpeed = speed;

                // CanCollect
                entity.CanCollect = e.CanCollect ?? false;

                // Animations
                entity.Animations = new Dictionary<Behavior.AnimationsNeeds, string>();
                if (e.Animations != null)
                {
                    foreach (var anim in e.Animations)
                    {
                        if (Enum.TryParse<Behavior.AnimationsNeeds>(anim.AnimationsNeeds, true, out var need))
                            entity.Animations[need] = anim.anim;
                    }
                }

                entity.AnimationsTextures = new Dictionary<string, Bitmap>();
                foreach (var anim in entity.Animations.Values)
                {
                    string animation_filename = Path.Combine(GlobalPaths.Animations, anim) + ".png";
                    using (Bitmap texture = File.Exists(animation_filename) ? (Bitmap)Image.FromFile(animation_filename) : new Bitmap(sz * 4, sz))
                        entity.AnimationsTextures[anim] = new Bitmap(texture);
                }

                result[e.EntityName] = entity;
            }

            return result;
        }
        public static void Save(string filePath, Dictionary<string, Entity> entities)
        {
            var root = new RootEntities
            {
                entities = new List<EntityData>()
            };

            foreach (var kvp in entities)
            {
                var name = kvp.Key;
                var e = kvp.Value;

                var data = new EntityData
                {
                    EntityName = name,
                    behaviors = e.Behaviors.ToList(),
                    alignment = e.Alignment.ToString(),
                    AnimationSpeed = e.AnimationSpeed + "F",
                    CanCollect = e.CanCollect,
                    Animations = e.Animations.Select(a => new AnimationEntry
                    {
                        AnimationsNeeds = a.Key.ToString(),
                        anim = a.Value
                    }).ToList()
                };

                root.entities.Add(data);

                // --- Sauvegarde des textures PNG ---
                if (e.AnimationsTextures != null)
                {
                    foreach (var tex in e.AnimationsTextures)
                    {
                        string filename = Path.Combine(GlobalPaths.Animations, tex.Key) + ".png";
                        tex.Value.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

            var json = JsonSerializer.Serialize(root, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }
    }
}
