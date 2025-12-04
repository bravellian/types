// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Bravellian;

public interface IDecimalBackedType<TSelf> : INumberBackedType<TSelf, decimal>
    where TSelf : INumberBackedType<TSelf, decimal>
{
}

public interface IDoubleBackedType<TSelf> : INumberBackedType<TSelf, double>
    where TSelf : INumberBackedType<TSelf, double>
{
}

public interface ILongBackedType<TSelf> : INumberBackedType<TSelf, long>
    where TSelf : INumberBackedType<TSelf, long>
{
}

public interface IInt32BackedType<TSelf> : INumberBackedType<TSelf, int>
    where TSelf : INumberBackedType<TSelf, int>
{
}

public interface IByteBackedType<TSelf> : INumberBackedType<TSelf, byte>
    where TSelf : INumberBackedType<TSelf, byte>
{
}

public interface IShortBackedType<TSelf> : INumberBackedType<TSelf, short>
    where TSelf : INumberBackedType<TSelf, short>
{
}
