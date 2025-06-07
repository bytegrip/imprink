'use client';

import { useUser } from "@auth0/nextjs-auth0";
import {useEffect, useState} from "react";

export default function Home() {
    const { user, error, isLoading } = useUser();
    const [accessToken, setAccessToken] = useState(null);

    useEffect(() => {
        const fetchAccessToken = async () => {
            if (user) {
                try {
                    const response = await fetch('/auth/access-token');
                    const v = await fetch('/token');
                    if (response.ok) {
                        const tokenData = await response.text();
                        setAccessToken(tokenData);
                    } else {
                        setAccessToken('Token not available');
                    }
                } catch (error) {
                    setAccessToken('Error fetching token');
                }
            }
        };

        fetchAccessToken().then(r => console.log(r));
    }, [user]);

    async function checkValidity() {
        const check = await fetch('https://impr.ink/api/api/User', {method: 'POST'});
    }

    if (isLoading) {
        return (
            <div className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 flex items-center justify-center">
                <div className="relative">
                    <div className="w-16 h-16 border-4 border-white/20 border-t-white rounded-full animate-spin"></div>
                    <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
                        <div className="w-10 h-10 border-4 border-transparent border-t-purple-400 rounded-full animate-spin"></div>
                    </div>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="min-h-screen bg-gradient-to-br from-red-900 via-pink-900 to-purple-900 flex items-center justify-center p-4">
                <div className="bg-white/10 backdrop-blur-xl rounded-2xl p-6 border border-white/20 shadow-2xl">
                    <div className="text-white/80 mb-4">{error.message}</div>
                    <div className="text-center">
                        <a
                            href="/auth/login"
                            className="group relative inline-flex items-center gap-2 px-8 py-3 bg-gradient-to-r from-purple-500 to-blue-500 rounded-xl font-bold text-white shadow-2xl hover:shadow-purple-500/25 transition-all duration-300 hover:scale-105 active:scale-95"
                        >
                            <div
                                className="absolute inset-0 bg-gradient-to-r from-purple-600 to-blue-600 rounded-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
                            <span className="relative flex items-center gap-2">
                                Sign In
                            </span>
                        </a>
                        <a
                            onClick={() => checkValidity()}
                            className="group relative px-6 py-3 bg-gradient-to-r from-red-500 to-pink-500 rounded-xl font-bold text-white shadow-2xl hover:shadow-red-500/25 transition-all duration-300 hover:scale-105 active:scale-95"
                        >
                            <div
                                className="absolute inset-0 bg-gradient-to-r from-red-600 to-pink-600 rounded-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
                            <span className="relative flex items-center gap-2">
                                    Check
                                </span>
                        </a>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div
            className="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 relative overflow-hidden">
            <div className="relative z-10 min-h-screen flex items-center justify-center p-4">
                {user ? (
                    <div className="w-full max-w-5xl">
                        <div className="text-center mb-6">
                            <div
                                className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-purple-500 to-blue-500 rounded-full mb-3 shadow-2xl">
                                {user.picture ? (
                                    <img
                                        src={user.picture}
                                        alt="Profile"
                                        className="w-full h-full rounded-full object-cover border-3 border-white/20"
                                    />
                                ) : (
                                    <div className="text-white text-xl font-bold">
                                        {user.name?.charAt(0) || user.email?.charAt(0) || '👤'}
                                    </div>
                                )}
                            </div>
                            <h1 className="text-2xl pb-1 font-bold bg-gradient-to-r from-white via-purple-200 to-blue-200 bg-clip-text text-transparent">
                                Just testing :P
                            </h1>
                        </div>

                        <div className="bg-white/10 backdrop-blur-xl rounded-2xl border border-white/20 shadow-2xl overflow-hidden mb-4">
                            <div className="bg-gradient-to-r from-purple-500/20 to-blue-500/20 p-4 border-b border-white/10">
                                <h2 className="text-xl font-bold text-white flex items-center gap-2">
                                    Auth Details
                                </h2>
                            </div>
                            <div className="p-5">
                                <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                                    <div className="space-y-4">
                                        <div>
                                            <label
                                                className="text-purple-300 text-xs font-semibold uppercase tracking-wider">Name</label>
                                            <div
                                                className="text-white text-base mt-1 p-2 bg-white/5 rounded-lg border border-white/10">
                                                {user.name || 'Not provided'}
                                            </div>
                                        </div>
                                        <div>
                                            <label
                                                className="text-purple-300 text-xs font-semibold uppercase tracking-wider">Email</label>
                                            <div
                                                className="text-white text-base mt-1 p-2 bg-white/5 rounded-lg border border-white/10">
                                                {user.email || 'Not provided'}
                                            </div>
                                        </div>
                                        <div>
                                            <label
                                                className="text-purple-300 text-xs font-semibold uppercase tracking-wider">User
                                                ID</label>
                                            <div
                                                className="text-white/80 text-xs mt-1 p-2 bg-white/5 rounded-lg border border-white/10 font-mono break-all">
                                                {user.sub || 'Not available'}
                                            </div>
                                        </div>
                                        {user.nickname && (
                                            <div>
                                                <label
                                                    className="text-purple-300 text-xs font-semibold uppercase tracking-wider">Nickname</label>
                                                <div
                                                    className="text-white text-base mt-1 p-2 bg-white/5 rounded-lg border border-white/10">
                                                    {user.nickname}
                                                </div>
                                            </div>
                                        )}
                                        <div>
                                            <label
                                                className="text-purple-300 text-xs font-semibold uppercase tracking-wider">Access
                                                Token</label>
                                            <div
                                                className="text-white/80 text-xs mt-1 p-2 bg-black/30 rounded-lg border border-white/10 font-mono break-all max-h-24 overflow-auto">
                                                {accessToken}
                                            </div>
                                        </div>
                                    </div>

                                    <div>
                                        <label
                                            className="text-purple-300 text-xs font-semibold uppercase tracking-wider mb-2 block">
                                            Raw User Data
                                        </label>
                                        <div
                                            className="bg-black/30 rounded-lg p-3 border border-white/10 h-64 overflow-auto">
                                            <pre
                                                className="text-green-300 text-xs font-mono leading-tight whitespace-pre-wrap">
                                                {JSON.stringify(user, null, 2)}
                                            </pre>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="flex justify-center">
                            <a
                                onClick={() => checkValidity()}
                                className="group relative px-6 py-3 bg-gradient-to-r from-red-500 to-pink-500 rounded-xl font-bold text-white shadow-2xl hover:shadow-red-500/25 transition-all duration-300 hover:scale-105 active:scale-95"
                            >
                                <div
                                    className="absolute inset-0 bg-gradient-to-r from-red-600 to-pink-600 rounded-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
                                <span className="relative flex items-center gap-2">
                                    Check
                                </span>
                            </a>
                            <a
                                href="/auth/logout"
                                className="group relative px-6 py-3 bg-gradient-to-r from-red-500 to-pink-500 rounded-xl font-bold text-white shadow-2xl hover:shadow-red-500/25 transition-all duration-300 hover:scale-105 active:scale-95"
                            >
                                <div
                                    className="absolute inset-0 bg-gradient-to-r from-red-600 to-pink-600 rounded-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
                                <span className="relative flex items-center gap-2">
                                    Sign Out
                                </span>
                            </a>
                        </div>
                    </div>
                ) : (
                    <div></div>
                )}
            </div>
        </div>
    );
}