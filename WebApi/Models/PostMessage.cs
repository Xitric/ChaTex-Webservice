/*
 * ChaTex Web API
 *
 * The Web API for ChaTex
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PostMessage :  IEquatable<PostMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostMessage" /> class.
        /// </summary>
        /// <param name="Content">Content (required).</param>
        /// <param name="Author">Author (required).</param>
        public PostMessage(string Content = null, long? Author = null)
        {
            // to ensure "Content" is required (not null)
            if (Content == null)
            {
                throw new InvalidDataException("Content is a required property for PostMessage and cannot be null");
            }
            else
            {
                this.Content = Content;
            }
            // to ensure "Author" is required (not null)
            if (Author == null)
            {
                throw new InvalidDataException("Author is a required property for PostMessage and cannot be null");
            }
            else
            {
                this.Author = Author;
            }
            
        }

        /// <summary>
        /// Gets or Sets Content
        /// </summary>
        [DataMember(Name="Content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or Sets Author
        /// </summary>
        [DataMember(Name="Author")]
        public long? Author { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PostMessage {\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  Author: ").Append(Author).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PostMessage)obj);
        }

        /// <summary>
        /// Returns true if PostMessage instances are equal
        /// </summary>
        /// <param name="other">Instance of PostMessage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PostMessage other)
        {

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    this.Content == other.Content ||
                    this.Content != null &&
                    this.Content.Equals(other.Content)
                ) && 
                (
                    this.Author == other.Author ||
                    this.Author != null &&
                    this.Author.Equals(other.Author)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.Content != null)
                    hash = hash * 59 + this.Content.GetHashCode();
                if (this.Author != null)
                    hash = hash * 59 + this.Author.GetHashCode();
                return hash;
            }
        }

        #region Operators

        public static bool operator ==(PostMessage left, PostMessage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PostMessage left, PostMessage right)
        {
            return !Equals(left, right);
        }

        #endregion Operators

    }
}
