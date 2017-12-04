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
    public partial class ChannelEventDTO :  IEquatable<ChannelEventDTO>
    {         /// <summary>
        /// The type of action that generated this event
        /// </summary>
        /// <value>The type of action that generated this event</value>
        public enum TypeEnum
        { 
            /// <summary>
            /// Enum NewMessageEnum for "NewMessage"
            /// </summary>
            [EnumMember(Value = "NewMessage")]
            NewMessageEnum = 1,
            
            /// <summary>
            /// Enum UpdateMessageEnum for "UpdateMessage"
            /// </summary>
            [EnumMember(Value = "UpdateMessage")]
            UpdateMessageEnum = 2,
            
            /// <summary>
            /// Enum DeleteMessageEnum for "DeleteMessage"
            /// </summary>
            [EnumMember(Value = "DeleteMessage")]
            DeleteMessageEnum = 3,
            
            /// <summary>
            /// Enum RenameChannelEnum for "RenameChannel"
            /// </summary>
            [EnumMember(Value = "RenameChannel")]
            RenameChannelEnum = 4,
            
            /// <summary>
            /// Enum DeleteChannelEnum for "DeleteChannel"
            /// </summary>
            [EnumMember(Value = "DeleteChannel")]
            DeleteChannelEnum = 5
        }

        /// <summary>
        /// The type of action that generated this event
        /// </summary>
        /// <value>The type of action that generated this event</value>
        [Required]
        [DataMember(Name="Type")]
        public TypeEnum? Type { get; set; }

        /// <summary>
        /// Gets or Sets TimeOfOccurrence
        /// </summary>
        [Required]
        [DataMember(Name="TimeOfOccurrence")]
        public DateTime? TimeOfOccurrence { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name="Message")]
        public GetMessageDTO Message { get; set; }

        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [DataMember(Name="Channel")]
        public ChannelDTO Channel { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ChannelEventDTO {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  TimeOfOccurrence: ").Append(TimeOfOccurrence).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Channel: ").Append(Channel).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ChannelEventDTO)obj);
        }

        /// <summary>
        /// Returns true if ChannelEventDTO instances are equal
        /// </summary>
        /// <param name="other">Instance of ChannelEventDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ChannelEventDTO other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) && 
                (
                    TimeOfOccurrence == other.TimeOfOccurrence ||
                    TimeOfOccurrence != null &&
                    TimeOfOccurrence.Equals(other.TimeOfOccurrence)
                ) && 
                (
                    Message == other.Message ||
                    Message != null &&
                    Message.Equals(other.Message)
                ) && 
                (
                    Channel == other.Channel ||
                    Channel != null &&
                    Channel.Equals(other.Channel)
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
                    if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                    if (TimeOfOccurrence != null)
                    hashCode = hashCode * 59 + TimeOfOccurrence.GetHashCode();
                    if (Message != null)
                    hashCode = hashCode * 59 + Message.GetHashCode();
                    if (Channel != null)
                    hashCode = hashCode * 59 + Channel.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ChannelEventDTO left, ChannelEventDTO right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChannelEventDTO left, ChannelEventDTO right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}