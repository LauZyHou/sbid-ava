using System;
using System.Collections.Generic;
using System.Text;

namespace sbid._T
{
    // 内置公理，在转换到ProVerif时将根据用户选择直接复制到结果中
    public class InnerAxiom
    {
        // 构造：对称加密。解构：对称解密。
        public const string SymEncDec =
            "(* ------------------------SymEncDec------------------------ *)\n" +
            "type key.\n" +
            "fun senc(bitstring, key): bitstring.\n" +
            "reduc forall m: bitstring, k: key; sdec(senc(m,k),k) = m.\n";

        // 构造：非对称加密。解构：非对称解密。
        public const string AsymEncDec =
            "(* ------------------------AsymEncDec------------------------ *)\n" +
            "type skey.\n" +
            "type pkey.\n" +
            "fun pk(skey): pkey.\n" +
            "fun aenc(bitstring, pkey): bitstring.\n" +
            "reduc forall m: bitstring, sk: skey; adec(aenc(m,pk(sk)),sk) = m.\n";

        // 构造：数字签名。解构：获取消息。解构：验证签名后获取消息。
        public const string DigitalSignature =
            "(* ------------------------DigitalSignature------------------------ *)\n" +
            "type sskey.\n" +
            "type spkey.\n" +
            "fun spk(sskey): spkey.\n" +
            "fun sign(bitstring, sskey): bitstring.\n" +
            "reduc forall m: bitstring, ssk: sskey; getmess(sign(m,ssk)) = m.\n" +
            "reduc forall m: bitstring, ssk: sskey; checksign(sign(m,ssk),spk(ssk)) = m.\n";

    }
}
