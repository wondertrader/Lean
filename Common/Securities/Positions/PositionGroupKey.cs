/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.Collections.Generic;
using QuantConnect.Util;

namespace QuantConnect.Securities.Positions
{
    /// <summary>
    /// Defines a unique and deterministic key for <see cref="IPositionGroup"/>
    /// </summary>
    public sealed class PositionGroupKey : IEquatable<PositionGroupKey>
    {
        /// <summary>
        /// Gets the <see cref="IPositionGroupBuyingPowerModel"/> being used by the group
        /// </summary>
        public IPositionGroupBuyingPowerModel BuyingPowerModel { get; }

        /// <summary>
        /// Gets the unit quantities defining the ratio between position quantities in the group
        /// </summary>
        public IReadOnlyList<Tuple<Symbol, decimal>> UnitQuantities { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionGroupKey"/> class for groups with a single security
        /// </summary>
        /// <param name="buyingPowerModel">The group's buying power model</param>
        /// <param name="security">The security</param>
        public PositionGroupKey(IPositionGroupBuyingPowerModel buyingPowerModel, Security security)
        {
            BuyingPowerModel = buyingPowerModel;
            UnitQuantities = new[]
            {
                Tuple.Create(security.Symbol, security.SymbolProperties.LotSize)
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionGroupKey"/> class
        /// </summary>
        /// <param name="buyingPowerModel">The group's buying power model</param>
        /// <param name="positions">The positions comprising the group</param>
        public PositionGroupKey(IPositionGroupBuyingPowerModel buyingPowerModel, IEnumerable<IPosition> positions)
        {
            BuyingPowerModel = buyingPowerModel;
            UnitQuantities = positions.ToList(p => Tuple.Create(p.Symbol, p.Quantity));
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(PositionGroupKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return UnitQuantities.Equals(other.UnitQuantities)
                && BuyingPowerModel.Equals(other.BuyingPowerModel);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is PositionGroupKey && Equals((PositionGroupKey) obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (BuyingPowerModel.GetHashCode() * 397) ^ UnitQuantities.GetHashCode();
            }
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        public static bool operator ==(PositionGroupKey left, PositionGroupKey right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        public static bool operator !=(PositionGroupKey left, PositionGroupKey right)
        {
            return !Equals(left, right);
        }
    }
}