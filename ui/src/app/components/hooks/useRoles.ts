import { useState, useEffect } from 'react';
import { useUser } from '@auth0/nextjs-auth0';
import clientApi from '@/lib/clientApi';

interface RoleResponse {
    roleName: string;
}

export const useRoles = () => {
    const { user } = useUser();
    const [roles, setRoles] = useState<string[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        const fetchUserRoles = async () => {
            if (!user) {
                setRoles([]);
                setError(null);
                return;
            }

            setIsLoading(true);
            setError(null);

            try {
                const response = await clientApi.get<RoleResponse[]>('/users/me/roles');
                const userRoles = response.data.map(role => role.roleName.toLowerCase());
                setRoles(userRoles);
            } catch (err) {
                console.error('Failed to fetch user roles:', err);
                setError(err as Error);
                setRoles([]);
            } finally {
                setIsLoading(false);
            }
        };

        fetchUserRoles().then(r => console.log(r));
    }, [user]);

    const hasRole = (roleName: string): boolean => {
        return roles.includes(roleName.toLowerCase());
    };

    const hasAnyRole = (roleNames: string[]): boolean => {
        return roleNames.some(roleName => hasRole(roleName));
    };

    const hasAllRoles = (roleNames: string[]): boolean => {
        return roleNames.every(roleName => hasRole(roleName));
    };

    const isMerchant = hasAnyRole(['merchant', 'admin']);
    const isAdmin = hasRole('admin');
    const isCustomer = hasRole('customer');

    return {
        roles,
        isLoading,
        error,
        hasRole,
        hasAnyRole,
        hasAllRoles,
        isMerchant,
        isAdmin,
        isCustomer,
    };
};

export default useRoles;
