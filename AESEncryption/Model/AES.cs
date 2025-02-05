﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AESEncryption.Model
{
    internal class AES
    {
        private const int BlockSize = 16;

        //public static byte[] Encrypt(string plainText, string key)
        //{
        //    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        //    byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        //    int numBlocks = (int)Math.Ceiling((double)plainBytes.Length / BlockSize);

        //    byte[] encryptedBytes = new byte[numBlocks * BlockSize];

        //    for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
        //    {
        //        byte[][] state = new byte[4][];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            state[i] = new byte[4];
        //        }

        //        for (int i = 0; i < BlockSize; i++)
        //        {
        //            if (blockIndex * BlockSize + i < plainBytes.Length)
        //            {
        //                state[i % 4][i / 4] = plainBytes[blockIndex * BlockSize + i];
        //            }
        //            else
        //            {
        //                // Padding: nếu độ dài dữ liệu không chia hết cho BlockSize, thêm byte 0
        //                state[i % 4][i / 4] = 0;
        //            }
        //        }

        //        byte[][] w = KeyExpansion(keyBytes);

        //        AddRoundKey(state, w, 0);

        //        for (int round = 1; round < 10; round++)
        //        {
        //            SubBytes(state);
        //            ShiftRows(state);
        //            MixColumns(state);
        //            AddRoundKey(state, w, round);
        //        }

        //        SubBytes(state);
        //        ShiftRows(state);
        //        AddRoundKey(state, w, 10);

        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                encryptedBytes[blockIndex * BlockSize + i * 4 + j] = state[j][i];
        //            }
        //        }
        //    }

        //    return encryptedBytes;
        //}
        public static void Encrypt(string inputFilePath, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] plainBytes = File.ReadAllBytes(inputFilePath);

            int numBlocks = (int)Math.Ceiling((double)plainBytes.Length / BlockSize);

            // Tạo tên file đích
            string outputFilePath = Path.GetFileNameWithoutExtension(inputFilePath) + "_encrypted" + Path.GetExtension(inputFilePath);

            using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
            {
                for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
                {
                    byte[][] state = new byte[4][];
                    for (int i = 0; i < 4; i++)
                    {
                        state[i] = new byte[4];
                    }

                    for (int i = 0; i < BlockSize; i++)
                    {
                        if (blockIndex * BlockSize + i < plainBytes.Length)
                        {
                            state[i % 4][i / 4] = plainBytes[blockIndex * BlockSize + i];
                        }
                        else
                        {
                            // Padding: nếu độ dài dữ liệu không chia hết cho BlockSize, thêm byte 0
                            state[i % 4][i / 4] = 0;
                        }
                    }

                    byte[][] w = KeyExpansion(keyBytes);

                    AddRoundKey(state, w, 0);

                    for (int round = 1; round < 10; round++)
                    {
                        SubBytes(state);
                        ShiftRows(state);
                        MixColumns(state);
                        AddRoundKey(state, w, round);
                    }

                    SubBytes(state);
                    ShiftRows(state);
                    AddRoundKey(state, w, 10);

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            outputFileStream.WriteByte(state[j][i]);
                        }
                    }
                }
            }

            Console.WriteLine("File đã được mã hóa và lưu vào: " + outputFilePath);
        }

        //public static string Decrypt(byte[] cipherText, string key)
        //{
        //    byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        //    int numBlocks = cipherText.Length / BlockSize;

        //    byte[] decryptedBytes = new byte[numBlocks * BlockSize];

        //    for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
        //    {
        //        byte[][] state = new byte[4][];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            state[i] = new byte[4];
        //        }

        //        for (int i = 0; i < BlockSize; i++)
        //        {
        //            state[i % 4][i / 4] = cipherText[blockIndex * BlockSize + i];
        //        }

        //        byte[][] w = KeyExpansion(keyBytes);

        //        AddRoundKey(state, w, 10);

        //        for (int round = 9; round > 0; round--)
        //        {
        //            InvShiftRows(state);
        //            InvSubBytes(state);
        //            AddRoundKey(state, w, round);
        //            InvMixColumns(state);
        //        }

        //        InvShiftRows(state);
        //        InvSubBytes(state);
        //        AddRoundKey(state, w, 0);

        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                decryptedBytes[blockIndex * BlockSize + i * 4 + j] = state[j][i];
        //            }
        //        }
        //    }

        //    // Xóa các byte 0 được thêm vào cuối block nếu có
        //    int paddingLength = decryptedBytes.Length;
        //    for (int i = decryptedBytes.Length - 1; i >= 0; i--)
        //    {
        //        if (decryptedBytes[i] == 0)
        //        {
        //            paddingLength--;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    Array.Resize(ref decryptedBytes, paddingLength);

        //    return Encoding.UTF8.GetString(decryptedBytes);
        //}

        public static void Decrypt(string inputFilePath, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] cipherBytes = File.ReadAllBytes(inputFilePath);

            int numBlocks = cipherBytes.Length / BlockSize;

            byte[] decryptedBytes = new byte[numBlocks * BlockSize];

            for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
            {
                byte[][] state = new byte[4][];
                for (int i = 0; i < 4; i++)
                {
                    state[i] = new byte[4];
                }

                for (int i = 0; i < BlockSize; i++)
                {
                    state[i % 4][i / 4] = cipherBytes[blockIndex * BlockSize + i];
                }

                byte[][] w = KeyExpansion(keyBytes);

                AddRoundKey(state, w, 10);

                for (int round = 9; round > 0; round--)
                {
                    InvShiftRows(state);
                    InvSubBytes(state);
                    AddRoundKey(state, w, round);
                    InvMixColumns(state);
                }

                InvShiftRows(state);
                InvSubBytes(state);
                AddRoundKey(state, w, 0);

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        decryptedBytes[blockIndex * BlockSize + i * 4 + j] = state[j][i];
                    }
                }
            }

            // Xóa các byte 0 được thêm vào cuối block nếu có
            int paddingLength = decryptedBytes.Length;
            for (int i = decryptedBytes.Length - 1; i >= 0; i--)
            {
                if (decryptedBytes[i] == 0)
                {
                    paddingLength--;
                }
                else
                {
                    break;
                }
            }

            Array.Resize(ref decryptedBytes, paddingLength);

            // Tạo tên file đích
            string outputFilePath = Path.GetFileNameWithoutExtension(inputFilePath) + "_decrypted" + Path.GetExtension(inputFilePath);

            // Ghi dữ liệu đã giải mã ra file mới
            File.WriteAllBytes(outputFilePath, decryptedBytes);

            Console.WriteLine("File đã được giải mã và lưu vào: " + outputFilePath);
        }
        private static void SubBytes(byte[][] state)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i][j] = SBox[state[i][j]];
                }
            }
        }

        private static void InvSubBytes(byte[][] state)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[i][j] = InvSBox[state[i][j]];
                }
            }
        }

        private static void ShiftRows(byte[][] state)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    byte temp = state[i][0];
                    for (int k = 0; k < 3; k++)
                    {
                        state[i][k] = state[i][k + 1];
                    }
                    state[i][3] = temp;
                }
            }
        }

        private static void InvShiftRows(byte[][] state)
        {
            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    byte temp = state[i][3];
                    for (int k = 3; k > 0; k--)
                    {
                        state[i][k] = state[i][k - 1];
                    }
                    state[i][0] = temp;
                }
            }
        }

        private static void MixColumns(byte[][] state)
        {
            byte[][] temp = new byte[4][];
            for (int i = 0; i < 4; i++)
            {
                temp[i] = new byte[4];
            }

            for (int i = 0; i < 4; i++)
            {
                temp[0][i] = (byte)(Multiply(0x02, state[0][i]) ^ Multiply(0x03, state[1][i]) ^ state[2][i] ^ state[3][i]);
                temp[1][i] = (byte)(state[0][i] ^ Multiply(0x02, state[1][i]) ^ Multiply(0x03, state[2][i]) ^ state[3][i]);
                temp[2][i] = (byte)(state[0][i] ^ state[1][i] ^ Multiply(0x02, state[2][i]) ^ Multiply(0x03, state[3][i]));
                temp[3][i] = (byte)(Multiply(0x03, state[0][i]) ^ state[1][i] ^ state[2][i] ^ Multiply(0x02, state[3][i]));
            }

            Array.Copy(temp, state, 4);
        }

        private static void InvMixColumns(byte[][] state)
        {
            byte[][] temp = new byte[4][];
            for (int i = 0; i < 4; i++)
            {
                temp[i] = new byte[4];
            }

            for (int i = 0; i < 4; i++)
            {
                temp[0][i] = (byte)(Multiply(0x0E, state[0][i]) ^ Multiply(0x0B, state[1][i]) ^ Multiply(0x0D, state[2][i]) ^ Multiply(0x09, state[3][i]));
                temp[1][i] = (byte)(Multiply(0x09, state[0][i]) ^ Multiply(0x0E, state[1][i]) ^ Multiply(0x0B, state[2][i]) ^ Multiply(0x0D, state[3][i]));
                temp[2][i] = (byte)(Multiply(0x0D, state[0][i]) ^ Multiply(0x09, state[1][i]) ^ Multiply(0x0E, state[2][i]) ^ Multiply(0x0B, state[3][i]));
                temp[3][i] = (byte)(Multiply(0x0B, state[0][i]) ^ Multiply(0x0D, state[1][i]) ^ Multiply(0x09, state[2][i]) ^ Multiply(0x0E, state[3][i]));
            }

            Array.Copy(temp, state, 4);
        }

        private static void AddRoundKey(byte[][] state, byte[][] w, int round)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[j][i] ^= w[round * 4 + i][j];
                }
            }
        }

        private static byte Multiply(byte a, byte b)
        {
            byte result = 0;
            byte highBit = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) == 1)
                {
                    result ^= a;
                }
                highBit = (byte)(a & 0x80);
                a <<= 1;
                if (highBit == 0x80)
                {
                    a ^= 0x1B;
                }
                b >>= 1;
            }
            return result;
        }

        private static byte[][] KeyExpansion(byte[] key)
        {
            int Nk = key.Length / 4;
            int Nr = Nk + 6;
            int Nb = 4;

            byte[][] w = new byte[Nb * (Nr + 1)][];

            for (int i = 0; i < Nk; i++)
            {
                byte[] temp = new byte[] { key[4 * i], key[4 * i + 1], key[4 * i + 2], key[4 * i + 3] };
                w[i] = temp;
            }

            for (int i = Nk; i < Nb * (Nr + 1); i++)
            {
                byte[] temp = new byte[4];
                Array.Copy(w[i - 1], temp, 4);
                if (i % Nk == 0)
                {
                    byte[] rotated = RotWord(temp);
                    byte[] subbed = SubWord(rotated);
                    byte rcon = Rcon[i / Nk];
                    subbed[0] ^= rcon;
                    temp = subbed;
                }
                else if (Nk > 6 && i % Nk == 4)
                {
                    temp = SubWord(temp);
                }
                for (int j = 0; j < 4; j++)
                {
                    temp[j] ^= w[i - Nk][j];
                }
                w[i] = temp;
            }

            return w;
        }

        private static byte[] RotWord(byte[] word)
        {
            byte temp = word[0];
            for (int i = 0; i < 3; i++)
            {
                word[i] = word[i + 1];
            }
            word[3] = temp;
            return word;
        }

        private static byte[] SubWord(byte[] word)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = SBox[word[i]];
            }
            return result;
        }
        private static readonly byte[] Rcon = {
        0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
        };
        // S-Box
        private static readonly byte[] SBox = {
        0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76,
        0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0,
        0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15,
        0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75,
        0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84,
        0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF,
        0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8,
        0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2,
        0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73,
        0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB,
        0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79,
        0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08,
        0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A,
        0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E,
        0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF,
        0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16
    };

        // Inverse S-Box
        private static readonly byte[] InvSBox = {
        0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB,
        0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB,
        0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E,
        0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25,
        0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92,
        0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84,
        0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06,
        0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B,
        0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73,
        0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E,
        0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B,
        0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4,
        0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F,
        0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF,
        0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61,
        0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D
    };
    }
}
