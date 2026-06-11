using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace WordHeapProject.Tests
{
    // ════════════════════════════════════════════════════════════════════════
    //  HeapNode Testleri
    // ════════════════════════════════════════════════════════════════════════
    public class HeapNodeTests
    {
        [Fact]
        public void HeapNode_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var node = new HeapNode("test");

            // Assert
            Assert.Equal("test", node.Word);
            Assert.Equal(1, node.Count);
        }

        [Fact]
        public void HeapNode_IncrementCount_UpdatesCorrectly()
        {
            // Arrange
            var node = new HeapNode("kelime");

            // Act
            node.Count++;
            node.Count++;

            // Assert
            Assert.Equal(3, node.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("abc")]
        [InlineData("UPPERCASE")]
        public void HeapNode_AcceptsAnyWord(string word)
        {
            // Arrange & Act
            var node = new HeapNode(word);

            // Assert
            Assert.Equal(word, node.Word);
            Assert.Equal(1, node.Count);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Temel İşlemler
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapBasicTests
    {
        [Fact]
        public void CustomHeap_IsEmpty_ReturnsTrueForNewHeap()
        {
            // Arrange & Act
            var heap = new CustomHeap();

            // Assert
            Assert.True(heap.IsEmpty());
        }

        [Fact]
        public void CustomHeap_Size_ReturnsCorrectCount()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("test");
            heap.ProcessWord("heap");
            heap.ProcessWord("word");

            // Assert
            Assert.Equal(3, heap.Size());
        }

        [Fact]
        public void CustomHeap_IsEmpty_ReturnsFalseAfterAddingWord()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("test");

            // Assert
            Assert.False(heap.IsEmpty());
        }

        [Fact]
        public void CustomHeap_ProcessWord_AddsSingleWord()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("kelime");

            // Assert
            Assert.Equal(1, heap.Size());
            Assert.False(heap.IsEmpty());
        }

        [Fact]
        public void CustomHeap_ProcessWord_IgnoresNullOrWhitespace()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord(null);
            heap.ProcessWord("");
            heap.ProcessWord("   ");

            // Assert
            Assert.Equal(0, heap.Size());
            Assert.True(heap.IsEmpty());
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Güncellemeler
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapUpdateTests
    {
        [Fact]
        public void CustomHeap_ProcessWord_UpdatesExistingWord()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("test");

            // Act
            heap.ProcessWord("test");
            heap.ProcessWord("test");

            // Assert
            Assert.Equal(1, heap.Size()); // Aynı kelime bir kez tutulur
            var node = heap.ExtractMin();
            Assert.Equal(3, node.Count); // Fakat count güncellenmiştir
        }

        [Fact]
        public void CustomHeap_ProcessWord_MaintainsSizeOnUpdate()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("word1");
            heap.ProcessWord("word2");
            heap.ProcessWord("word3");
            int initialSize = heap.Size();

            // Act
            heap.ProcessWord("word2"); // Mevcut kelimeyi güncelle

            // Assert
            Assert.Equal(initialSize, heap.Size()); // Size değişmemeli
        }

        [Fact]
        public void CustomHeap_ProcessWord_IncrementFrequency()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("word");

            // Act
            for (int i = 0; i < 5; i++)
                heap.ProcessWord("word");

            // Assert
            var node = heap.ExtractMin();
            Assert.Equal(6, node.Count); // 1 başlangıç + 5 güncelleme
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - ExtractMin ve Heap Düzeni
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapExtractionTests
    {
        [Fact]
        public void CustomHeap_ExtractMin_ReturnsNullForEmptyHeap()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            var result = heap.ExtractMin();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CustomHeap_ExtractMin_ReturnsSingleElement()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("test");

            // Act
            var result = heap.ExtractMin();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Word);
            Assert.True(heap.IsEmpty());
        }

        [Fact]
        public void CustomHeap_ExtractMin_RemovesFromHeap()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("word1");
            heap.ProcessWord("word2");
            int initialSize = heap.Size();

            // Act
            heap.ExtractMin();

            // Assert
            Assert.Equal(initialSize - 1, heap.Size());
        }

        [Fact]
        public void CustomHeap_ExtractMin_EmptiesHeap()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("a");
            heap.ProcessWord("b");
            heap.ProcessWord("c");

            // Act
            heap.ExtractMin();
            heap.ExtractMin();
            heap.ExtractMin();

            // Assert
            Assert.True(heap.IsEmpty());
            Assert.Null(heap.ExtractMin());
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - İlk Harf Anahtarı (Anahtar 1)
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapFirstLetterKeyTests
    {
        [Fact]
        public void CustomHeap_FirstLetterKey_OrdersAlphabetically()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("banana");
            heap.ProcessWord("cherry");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();
            var result3 = heap.ExtractMin();

            // Assert
            Assert.Equal('a', result1.Word[0]); // apple
            Assert.Equal('b', result2.Word[0]); // banana
            Assert.Equal('c', result3.Word[0]); // cherry
        }

        [Fact]
        public void CustomHeap_FirstLetterKey_LowercaseComparison()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("Apple");
            heap.ProcessWord("banana");
            heap.ProcessWord("Cherry");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();
            var result3 = heap.ExtractMin();

            // Assert
            // Tüm kelimeler türkçe kültürüne göre küçük harfe çevrilir
            Assert.Equal("apple", result1.Word);
            Assert.Equal("banana", result2.Word);
            Assert.Equal("cherry", result3.Word);
        }

        [Fact]
        public void CustomHeap_FirstLetterKey_WithMixedLetters()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("zebra");
            heap.ProcessWord("apple");
            heap.ProcessWord("mouse");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();
            var result3 = heap.ExtractMin();

            // Assert
            Assert.Equal('a', result1.Word[0]);
            Assert.Equal('m', result2.Word[0]);
            Assert.Equal('z', result3.Word[0]);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Frekans Anahtarı (Anahtar 2)
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapFrequencyKeyTests
    {
        [Fact]
        public void CustomHeap_FrequencyKey_HigherFrequencyComesFirst()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("apricot");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();

            // Assert
            Assert.Equal("apple", result1.Word);
            Assert.Equal(4, result1.Count);
            Assert.Equal("apricot", result2.Word);
            Assert.Equal(1, result2.Count);
        }

        [Fact]
        public void CustomHeap_FrequencyKey_DifferentFirstLetters()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();

            // Assert
            // 'a' ilk harfi 'b'den küçük, bu yüzden 'apple' ilk çıkar
            Assert.Equal("apple", result1.Word);
            Assert.Equal("banana", result2.Word);
        }

        [Fact]
        public void CustomHeap_FrequencyKey_SameFirstLetterHigherFrequencyFirst()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("apricot");
            heap.ProcessWord("apricot");
            heap.ProcessWord("apricot");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();

            // Assert
            // Aynı ilk harf 'a', frekansa göre sıralı
            Assert.Equal("apricot", result1.Word);
            Assert.Equal(3, result1.Count);
            Assert.Equal("apple", result2.Word);
            Assert.Equal(2, result2.Count);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Bağ Kırıcı (Tie-Breaker)
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapTieBreakerTests
    {
        [Fact]
        public void CustomHeap_TieBreaker_SameFrequencyAlphabeticOrder()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("apricot");
            heap.ProcessWord("apricot");
            heap.ProcessWord("avocado");
            heap.ProcessWord("avocado");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();
            var result3 = heap.ExtractMin();

            // Assert
            // Tüm kelimelerin ilk harfi 'a' ve frekansı 2
            // Bağ kırıcısı kelime alfabetik sırasıdır
            Assert.Equal("apple", result1.Word);
            Assert.Equal("apricot", result2.Word);
            Assert.Equal("avocado", result3.Word);
        }

        [Fact]
        public void CustomHeap_TieBreaker_ComplexScenario()
        {
            // Arrange
            var heap = new CustomHeap();

            // 'b' harfi ile 2 kelime, her ikisinin frekansı 3
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");

            heap.ProcessWord("berry");
            heap.ProcessWord("berry");
            heap.ProcessWord("berry");

            // Act
            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();

            // Assert
            // İlk harf aynı 'b', frekans aynı 3
            // Bağ kırıcı alfabetik sıra
            Assert.Equal("banana", result1.Word);
            Assert.Equal("berry", result2.Word);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - PeekTopFrequency
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapPeekTopFrequencyTests
    {
        [Fact]
        public void CustomHeap_PeekTopFrequency_ReturnsNullForEmptyHeap()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            var result = heap.PeekTopFrequency();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CustomHeap_PeekTopFrequency_ReturnsMostFrequentWord()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("rare");
            heap.ProcessWord("common");
            heap.ProcessWord("common");
            heap.ProcessWord("common");
            heap.ProcessWord("common");

            // Act
            var result = heap.PeekTopFrequency();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("common", result.Word);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void CustomHeap_PeekTopFrequency_DoesNotModifyHeap()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("word");
            heap.ProcessWord("word");
            int initialSize = heap.Size();

            // Act
            var result1 = heap.PeekTopFrequency();
            var result2 = heap.PeekTopFrequency();
            int finalSize = heap.Size();

            // Assert
            Assert.Equal(initialSize, finalSize);
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }

        [Fact]
        public void CustomHeap_PeekTopFrequency_WithMultipleWords()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("a");
            heap.ProcessWord("a");

            heap.ProcessWord("b");
            heap.ProcessWord("b");
            heap.ProcessWord("b");

            heap.ProcessWord("c");
            heap.ProcessWord("c");
            heap.ProcessWord("c");
            heap.ProcessWord("c");
            heap.ProcessWord("c");

            // Act
            var result = heap.PeekTopFrequency();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("c", result.Word);
            Assert.Equal(5, result.Count);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Türkçe Karakterler
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapTurkishCharacterTests
    {
        [Fact]
        public void CustomHeap_TurkishCharacters_ProcessesTurkishWords()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("kedi");
            heap.ProcessWord("köpek");
            heap.ProcessWord("kuş");

            // Assert
            Assert.Equal(3, heap.Size());
            Assert.False(heap.IsEmpty());
        }

        [Fact]
        public void CustomHeap_TurkishCharacters_LowercaseConversionWithTurkishCulture()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("İstanbul");
            heap.ProcessWord("istanbul");

            // Assert
            // Türkçe kültürüne göre İ -> i dönüşümü yapılır
            Assert.Equal(1, heap.Size()); // Aynı kelime olarak tutulmalı
            var node = heap.ExtractMin();
            Assert.Equal(2, node.Count);
        }

        [Fact]
        public void CustomHeap_TurkishCharacters_SpecialCharacterHandling()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("çalışma");
            heap.ProcessWord("şehir");
            heap.ProcessWord("ürün");

            // Assert
            Assert.Equal(3, heap.Size());
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  Program.CleanWord Testleri
    // ════════════════════════════════════════════════════════════════════════
    public class ProgramCleanWordTests
    {
        [Fact]
        public void CleanWord_RemovesNonLetterCharacters()
        {
            // Arrange & Act
            string result = Program.CleanWord("hello123");

            // Assert
            Assert.Equal("hello", result);
        }

        [Fact]
        public void CleanWord_RemovesPunctuation()
        {
            // Arrange & Act
            string result = Program.CleanWord("hello,world!");

            // Assert
            Assert.Equal("helloworld", result);
        }

        [Fact]
        public void CleanWord_RemovesNumbers()
        {
            // Arrange & Act
            string result = Program.CleanWord("abc123def456");

            // Assert
            Assert.Equal("abcdef", result);
        }

        [Fact]
        public void CleanWord_HandlesEmptyString()
        {
            // Arrange & Act
            string result = Program.CleanWord("");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CleanWord_HandlesOnlyNumbers()
        {
            // Arrange & Act
            string result = Program.CleanWord("123456");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CleanWord_HandlesOnlyPunctuation()
        {
            // Arrange & Act
            string result = Program.CleanWord("!@#$%^&*");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void CleanWord_KeepsLettersOnly()
        {
            // Arrange & Act
            string result = Program.CleanWord("hello-world_123!");

            // Assert
            Assert.Equal("helloworld", result);
        }

        [Fact]
        public void CleanWord_HandlesUppercaseLetters()
        {
            // Arrange & Act
            string result = Program.CleanWord("HeLLo123WoRLD");

            // Assert
            Assert.Equal("HeLLoWoRLD", result);
        }

        [Fact]
        public void CleanWord_HandlesTurkishCharacters()
        {
            // Arrange & Act
            string result = Program.CleanWord("mü.şteri@123");

            // Assert
            Assert.Equal("müşteri", result);
        }

        [Theory]
        [InlineData("word", "word")]
        [InlineData("w0rd", "wrd")]
        [InlineData("w@rd", "wrd")]
        [InlineData("123", "")]
        [InlineData("W0RD!", "WRD")]
        public void CleanWord_Theory_VariousInputs(string input, string expected)
        {
            // Arrange & Act
            string result = Program.CleanWord(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Verbose Mod
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapVerboseModeTests
    {
        [Fact]
        public void CustomHeap_VerboseMode_CanBeToggled()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act & Assert
            Assert.False(heap.Verbose);
            heap.Verbose = true;
            Assert.True(heap.Verbose);
            heap.Verbose = false;
            Assert.False(heap.Verbose);
        }

        [Fact]
        public void CustomHeap_VerboseMode_DoesNotAffectFunctionality()
        {
            // Arrange
            var heap1 = new CustomHeap { Verbose = false };
            var heap2 = new CustomHeap { Verbose = true };

            // Act
            heap1.ProcessWord("test");
            heap1.ProcessWord("test");

            heap2.ProcessWord("test");
            heap2.ProcessWord("test");

            var result1 = heap1.ExtractMin();
            var result2 = heap2.ExtractMin();

            // Assert
            Assert.Equal(result1.Word, result2.Word);
            Assert.Equal(result1.Count, result2.Count);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CustomHeap Testleri - Kompleks Senaryolar
    // ════════════════════════════════════════════════════════════════════════
    public class CustomHeapComplexScenarioTests
    {
        [Fact]
        public void CustomHeap_LargeDataSet_ProcessesCorrectly()
        {
            // Arrange
            var heap = new CustomHeap();
            string[] words = { "apple", "banana", "apple", "cherry", "banana", "apple" };

            // Act
            foreach (var word in words)
                heap.ProcessWord(word);

            // Assert
            Assert.Equal(3, heap.Size()); // 3 unique words
        }

        [Fact]
        public void CustomHeap_ExtractAll_MaintainsOrder()
        {
            // Arrange
            var heap = new CustomHeap();
            heap.ProcessWord("apple");
            heap.ProcessWord("apple");
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");
            heap.ProcessWord("banana");
            heap.ProcessWord("cherry");

            var extractedWords = new List<string>();

            // Act
            while (!heap.IsEmpty())
            {
                var node = heap.ExtractMin();
                extractedWords.Add(node.Word);
            }

            // Assert
            // First letter order: a, b, c
            Assert.Equal("apple", extractedWords[0]);
            Assert.Equal("banana", extractedWords[1]);
            Assert.Equal("cherry", extractedWords[2]);
        }

        [Fact]
        public void CustomHeap_MixedOperations_StaysConsistent()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("word1");
            heap.ProcessWord("word2");
            heap.ProcessWord("word1");
            var top1 = heap.PeekTopFrequency();

            heap.ProcessWord("word3");
            heap.ProcessWord("word3");
            heap.ProcessWord("word3");
            var top2 = heap.PeekTopFrequency();

            // Assert
            Assert.NotNull(top1);
            Assert.NotNull(top2);
            Assert.Equal("word3", top2.Word);
        }

        [Fact]
        public void CustomHeap_DuplicateProcessing_CountsCorrectly()
        {
            // Arrange
            var heap = new CustomHeap();
            string word = "duplicate";

            // Act
            for (int i = 0; i < 10; i++)
                heap.ProcessWord(word);

            var result = heap.ExtractMin();

            // Assert
            Assert.Equal(0, heap.Size()); // Should be empty after extracting the only word
            Assert.Equal(word, result.Word);
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public void CustomHeap_CaseInsensitivity_WorksCorrectly()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("Word");
            heap.ProcessWord("WORD");
            heap.ProcessWord("word");

            // Assert
            Assert.Equal(1, heap.Size()); // All variations should be same word
            var result = heap.ExtractMin();
            Assert.Equal(3, result.Count);
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  Edge Case Testleri
    // ════════════════════════════════════════════════════════════════════════
    public class EdgeCaseTests
    {
        [Fact]
        public void EdgeCase_SingleCharacterWords()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("a");
            heap.ProcessWord("b");
            heap.ProcessWord("c");
            heap.ProcessWord("a");
            heap.ProcessWord("a");

            var result1 = heap.ExtractMin();
            var result2 = heap.ExtractMin();
            var result3 = heap.ExtractMin();

            // Assert
            Assert.Equal("a", result1.Word);
            Assert.Equal(3, result1.Count);
        }

        [Fact]
        public void EdgeCase_VeryLongWord()
        {
            // Arrange
            var heap = new CustomHeap();
            string longWord = new string('a', 1000);

            // Act
            heap.ProcessWord(longWord);
            var result = heap.ExtractMin();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Word.Length);
        }

        [Fact]
        public void EdgeCase_SpecialUnicodeCharacters()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord("café");
            heap.ProcessWord("naïve");
            heap.ProcessWord("résumé");

            // Assert
            Assert.Equal(3, heap.Size());
        }

        [Fact]
        public void EdgeCase_WhitespaceVariations()
        {
            // Arrange
            var heap = new CustomHeap();

            // Act
            heap.ProcessWord(" ");
            heap.ProcessWord("\t");
            heap.ProcessWord("\n");

            // Assert
            Assert.Equal(0, heap.Size()); // All whitespace should be ignored
        }

        [Fact]
        public void CleanWord_MixedContent_RemovesCorrectly()
        {
            // Arrange & Act
            string result = Program.CleanWord("h3ll0-w0rld!");

            // Assert
            Assert.Equal("hllwrld", result);
        }
    }
}
