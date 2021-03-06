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
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CreateGroupDTO :  IEquatable<CreateGroupDTO>
    { 
        /// <summary>
        /// Gets or Sets GroupName
        /// </summary>
        [Required]
        [DataMember(Name="GroupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or Sets AllowEmployeeSticky
        /// </summary>
        [Required]
        [DataMember(Name="AllowEmployeeSticky")]
        public bool? AllowEmployeeSticky { get; set; }

        /// <summary>
        /// Gets or Sets AllowEmployeeAcknowledgeable
        /// </summary>
        [Required]
        [DataMember(Name="AllowEmployeeAcknowledgeable")]
        public bool? AllowEmployeeAcknowledgeable { get; set; }

        /// <summary>
        /// Gets or Sets AllowEmployeeBookmark
        /// </summary>
        [Required]
        [DataMember(Name="AllowEmployeeBookmark")]
        public bool? AllowEmployeeBookmark { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CreateGroupDTO {\n");
            sb.Append("  GroupName: ").Append(GroupName).Append("\n");
            sb.Append("  AllowEmployeeSticky: ").Append(AllowEmployeeSticky).Append("\n");
            sb.Append("  AllowEmployeeAcknowledgeable: ").Append(AllowEmployeeAcknowledgeable).Append("\n");
            sb.Append("  AllowEmployeeBookmark: ").Append(AllowEmployeeBookmark).Append("\n");
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
            return obj.GetType() == GetType() && Equals((CreateGroupDTO)obj);
        }

        /// <summary>
        /// Returns true if CreateGroupDTO instances are equal
        /// </summary>
        /// <param name="other">Instance of CreateGroupDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CreateGroupDTO other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    GroupName == other.GroupName ||
                    GroupName != null &&
                    GroupName.Equals(other.GroupName)
                ) && 
                (
                    AllowEmployeeSticky == other.AllowEmployeeSticky ||
                    AllowEmployeeSticky != null &&
                    AllowEmployeeSticky.Equals(other.AllowEmployeeSticky)
                ) && 
                (
                    AllowEmployeeAcknowledgeable == other.AllowEmployeeAcknowledgeable ||
                    AllowEmployeeAcknowledgeable != null &&
                    AllowEmployeeAcknowledgeable.Equals(other.AllowEmployeeAcknowledgeable)
                ) && 
                (
                    AllowEmployeeBookmark == other.AllowEmployeeBookmark ||
                    AllowEmployeeBookmark != null &&
                    AllowEmployeeBookmark.Equals(other.AllowEmployeeBookmark)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (GroupName != null)
                    hashCode = hashCode * 59 + GroupName.GetHashCode();
                    if (AllowEmployeeSticky != null)
                    hashCode = hashCode * 59 + AllowEmployeeSticky.GetHashCode();
                    if (AllowEmployeeAcknowledgeable != null)
                    hashCode = hashCode * 59 + AllowEmployeeAcknowledgeable.GetHashCode();
                    if (AllowEmployeeBookmark != null)
                    hashCode = hashCode * 59 + AllowEmployeeBookmark.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(CreateGroupDTO left, CreateGroupDTO right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CreateGroupDTO left, CreateGroupDTO right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
