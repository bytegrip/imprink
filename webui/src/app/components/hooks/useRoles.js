import { useState, useEffect } from 'react';
import { useUser } from '@auth0/nextjs-auth0';
import clientApi from '@/lib/clientApi';

export const useRoles = () => {
    const { user } = useUser();
    const [roles, setRoles] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

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
                const response = await clientApi.get('/users/me/roles');
                const userRoles = response.data.map(role => role.roleName.toLowerCase());
                setRoles(userRoles);
            } catch (err) {
                console.error('Failed to fetch user roles:', err);
                setError(err);
                setRoles([]);
            } finally {
                setIsLoading(false);
            }
        };

        fetchUserRoles().then(r => console.log(r));
    }, [user]);

    const hasRole = (roleName) => {
        return roles.includes(roleName.toLowerCase());
    };

    const hasAnyRole = (roleNames) => {
        return roleNames.some(roleName => hasRole(roleName));
    };

    const hasAllRoles = (roleNames) => {
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
        isCustomer
    };
};

export default useRoles;